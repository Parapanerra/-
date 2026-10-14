const app = document.getElementById('app');
const dialog = document.getElementById('delete-dialog');
const deliveryNames = {1: 'Відділення', 2: 'Кур’єр', 3: 'Експрес-кур’єр'};
const state = {postings: [], selectedId: null, busy: false};
const number = value => new Intl.NumberFormat('uk-UA', {maximumFractionDigits: 2}).format(value);
const money = value => new Intl.NumberFormat('uk-UA', {minimumFractionDigits: 2, maximumFractionDigits: 2}).format(value);
const escapeHtml = value => String(value ?? '').replace(/[&<>"']/g, char => ({'&':'&amp;','<':'&lt;','>':'&gt;','"':'&quot;',"'":'&#39;'}[char]));

async function api(path, options = {}) {
  let response;
  try { response = await fetch(path, options); }
  catch { throw new Error('Немає з’єднання із сервером PostService. Перевірте, чи його запущено.'); }
  if (!response.ok) {
    let details = '';
    try {
      const data = await response.json();
      details = data.detail || data.title || (data.errors ? Object.values(data.errors).flat().join(' ') : '');
    } catch { details = await response.text().catch(() => ''); }
    const fallback = response.status === 404 ? 'Запис не знайдено.' : `Помилка сервера (${response.status}).`;
    throw new Error(details || fallback);
  }
  return response.status === 204 ? null : response.json();
}

function toast(message) {
  const element = document.getElementById('toast');
  element.textContent = message;
  element.classList.add('show');
  clearTimeout(toast.timer);
  toast.timer = setTimeout(() => element.classList.remove('show'), 4000);
}

function setNav(page) {
  document.querySelectorAll('.nav a').forEach(link => {
    const active = link.dataset.route === page;
    link.classList.toggle('active', active);
    if (active) link.setAttribute('aria-current', 'page'); else link.removeAttribute('aria-current');
  });
  document.getElementById('nav').classList.remove('open');
  document.getElementById('menu-toggle').setAttribute('aria-expanded', 'false');
}

function route() {
  const path = location.hash.slice(1) || '/';
  if (path === '/') { setNav('home'); renderHome(); }
  else if (path === '/about') { setNav('about'); renderAbout(); }
  else if (path === '/postings') { setNav('postings'); renderList(); }
  else if (path === '/postings/new') { setNav('postings'); renderForm(); }
  else {
    const match = path.match(/^\/postings\/(\d+)(?:\/(edit))?$/);
    if (match) { setNav('postings'); match[2] ? renderForm(Number(match[1])) : renderDetails(Number(match[1])); }
    else { location.hash = '#/'; return; }
  }
  window.scrollTo(0, 0);
}

function renderHome() {
  app.innerHTML = `<section class="home"><h1>Керування поштовими відправленнями</h1><p class="muted">Оберіть розділ для роботи.</p><div class="home-actions"><a class="button primary" href="#/postings">Список відправлень</a><a class="button" href="#/about">Про програму</a></div></section>`;
}

function renderAbout() {
  app.innerHTML = `<section class="about"><h1>Про програму</h1><h2>PostService</h2><p>Клієнтський застосунок для роботи з поштовими відправленнями.</p><p>Перегляд, створення, редагування та видалення записів.<br>Дані зберігає сервер PostService через REST API.</p><p class="footnote muted">Навчальний проєкт. Адаптивний вебінтерфейс.</p><a class="button primary" href="#/">На головну</a></section>`;
}

async function renderList() {
  app.innerHTML = `<section class="list-page"><h1>Список поштових відправлень</h1><div class="toolbar"><a class="button primary" href="#/postings/new">Нове відправлення</a><button class="button" id="edit-button" disabled>Редагувати</button><button class="button" id="details-button" disabled>Деталі</button><button class="button" id="delete-button" disabled>Видалити</button><span class="spacer"></span><button class="button" id="refresh-button">Оновити</button></div><div class="table-wrap"><table><thead><tr><th>ID</th><th>Відправник</th><th>Отримувач</th><th>Тип доставки</th><th>Вага, кг</th><th>Вартість, грн</th></tr></thead><tbody id="posting-rows"></tbody></table></div><div class="cards" id="posting-cards"></div><p class="status" id="list-status" role="status">Завантаження відправлень…</p></section>`;
  app.querySelector('#refresh-button').addEventListener('click', loadList);
  app.querySelector('#edit-button').addEventListener('click', () => location.hash = `#/postings/${state.selectedId}/edit`);
  app.querySelector('#details-button').addEventListener('click', () => location.hash = `#/postings/${state.selectedId}`);
  app.querySelector('#delete-button').addEventListener('click', () => confirmDelete(state.selectedId));
  await loadList();
}

async function loadList() {
  const status = app.querySelector('#list-status');
  if (!status) return;
  status.classList.remove('error');
  status.textContent = 'Завантаження відправлень…';
  try {
    state.postings = await api('/postings');
    if (!state.postings.some(item => item.id === state.selectedId)) state.selectedId = null;
    drawList();
    status.textContent = state.postings.length ? `Готово. Відправлень: ${state.postings.length}` : 'Відправлень поки немає. Створіть перше відправлення.';
  } catch (error) {
    state.postings = [];
    drawList();
    status.classList.add('error');
    status.textContent = `Не вдалося завантажити список. ${error.message}`;
  }
}

function drawList() {
  const rows = app.querySelector('#posting-rows');
  const cards = app.querySelector('#posting-cards');
  if (!rows || !cards) return;
  rows.innerHTML = state.postings.map(item => `<tr data-id="${item.id}" tabindex="0" aria-selected="${item.id === state.selectedId}" class="${item.id === state.selectedId ? 'selected' : ''}"><td>${item.id}</td><td>${escapeHtml(item.from)}</td><td>${escapeHtml(item.to)}</td><td>${deliveryNames[item.deliveryType] || 'Невідомо'}</td><td>${number(item.weight)}</td><td>${money(item.price)}</td></tr>`).join('');
  cards.innerHTML = state.postings.map(item => `<article class="card ${item.id === state.selectedId ? 'selected' : ''}" data-id="${item.id}"><h2>ID ${item.id}</h2><p>Від: ${escapeHtml(item.from)}</p><p>Кому: ${escapeHtml(item.to)}</p><p class="muted">${deliveryNames[item.deliveryType] || 'Невідомо'} / ${number(item.weight)} кг</p><p>${money(item.price)} грн</p><div class="card-actions"><button class="text-button" data-action="details">Деталі</button><button class="text-button" data-action="edit">Редагувати</button><button class="text-button" data-action="delete">Видалити</button></div></article>`).join('');
  rows.querySelectorAll('tr').forEach(row => {
    row.addEventListener('click', () => select(Number(row.dataset.id)));
    row.addEventListener('dblclick', () => location.hash = `#/postings/${row.dataset.id}`);
    row.addEventListener('keydown', event => { if (event.key === 'Enter' || event.key === ' ') { event.preventDefault(); select(Number(row.dataset.id)); } });
  });
  cards.querySelectorAll('.card').forEach(card => {
    card.addEventListener('click', event => {
      const id = Number(card.dataset.id);
      select(id);
      const action = event.target.closest('[data-action]')?.dataset.action;
      if (action === 'details') location.hash = `#/postings/${id}`;
      if (action === 'edit') location.hash = `#/postings/${id}/edit`;
      if (action === 'delete') confirmDelete(id);
    });
  });
  ['edit-button','details-button','delete-button'].forEach(id => app.querySelector(`#${id}`).disabled = state.selectedId === null);
}

function select(id) { state.selectedId = id; drawList(); }

async function confirmDelete(id) {
  if (state.busy || id == null) return;
  document.getElementById('delete-message').textContent = `Видалити відправлення з ID ${id}?`;
  dialog.showModal();
  const result = await new Promise(resolve => dialog.addEventListener('close', () => resolve(dialog.returnValue), {once:true}));
  if (result !== 'delete') return;
  state.busy = true;
  try {
    await api(`/postings/${id}`, {method:'DELETE'});
    state.selectedId = null;
    toast(`Відправлення ${id} видалено.`);
    await loadList();
  } catch (error) { toast(`Не вдалося видалити: ${error.message}`); }
  finally { state.busy = false; }
}

async function renderDetails(id) {
  app.innerHTML = '<p class="status">Завантаження відправлення…</p>';
  try {
    const item = await api(`/postings/${id}`);
    const entries = [['ID',item.id],['Відправник',item.from],['Отримувач',item.to],['Вміст',item.content],['Тип доставки',deliveryNames[item.deliveryType] || 'Невідомо'],['Вага, кг',number(item.weight)],['Ширина, см',item.width],['Висота, см',item.height],['Глибина, см',item.depth],['Оголошена цінність, грн',item.value == null ? '—' : money(item.value)],['Вартість, грн',money(item.price)],['Опис',item.description || '—'],['Створено (UTC)',new Date(item.createdAt).toLocaleString('uk-UA',{timeZone:'UTC'})]];
    app.innerHTML = `<section class="details"><h1>Деталі відправлення</h1><dl>${entries.map(([label,value]) => `<div class="detail-row"><dt>${label}</dt><dd>${escapeHtml(value)}</dd></div>`).join('')}</dl><a class="button" href="#/postings">Назад до списку</a></section>`;
  } catch (error) { app.innerHTML = `<h1>Деталі відправлення</h1><p class="status error">${escapeHtml(error.message)}</p><a class="button" href="#/postings">Назад до списку</a>`; }
}

const fields = [
  ['from','Відправник *','text'],['to','Отримувач *','text'],['content','Вміст *','text'],
  ['deliveryType','Тип доставки *','select'],['weight','Вага, кг *','text'],
  ['width','Ширина, см *','text'],['height','Висота, см *','text'],['depth','Глибина, см *','text'],
  ['value','Оголошена цінність, грн','text'],['description','Опис','text']
];

async function renderForm(id = null) {
  const editing = id !== null;
  app.innerHTML = '<p class="status">Завантаження форми…</p>';
  let item = {};
  if (editing) {
    try { item = await api(`/postings/${id}`); }
    catch (error) { app.innerHTML = `<h1>Редагування відправлення</h1><p class="status error">${escapeHtml(error.message)}</p><a class="button" href="#/postings">Назад до списку</a>`; return; }
  }
  app.innerHTML = `<section class="form-page"><h1>${editing ? 'Редагування відправлення' : 'Нове відправлення'}</h1><p class="form-note">Поля зі знаком * обов’язкові.</p><p class="form-error" id="form-error" role="alert"></p><form class="posting-form" id="posting-form" novalidate>${fields.map(([name,label,type]) => `<div class="field"><label for="${name}">${label}</label>${type === 'select' ? `<select id="${name}" name="${name}"><option value="">Оберіть тип доставки</option><option value="1">Відділення</option><option value="2">Кур’єр</option><option value="3">Експрес-кур’єр</option></select>` : `<input id="${name}" name="${name}" type="text" value="${escapeHtml(item[name] ?? '')}" ${['weight','width','height','depth','value'].includes(name) ? 'inputmode="decimal"' : ''}>`}<span class="field-error" id="error-${name}"></span></div>`).join('')}<div class="field"><label for="price">Вартість, грн</label><input id="price" readonly value="${editing ? money(item.price) : 'Обчислить сервер після збереження'}"><span></span></div><div class="form-actions"><button type="submit" class="button primary" id="save-button">Зберегти</button><a class="button" href="#/postings">Скасувати</a></div></form></section>`;
  if (editing) app.querySelector('#deliveryType').value = String(item.deliveryType);
  const form = app.querySelector('#posting-form');
  form.addEventListener('submit', event => saveForm(event, id));
  form.addEventListener('input', event => { const control = event.target; control.classList.remove('invalid'); const error = app.querySelector(`#error-${control.name}`); if (error) error.textContent = ''; });
}

function readAndValidate(form) {
  const raw = Object.fromEntries(new FormData(form));
  const errors = {};
  for (const name of ['from','to','content']) if (!raw[name].trim()) errors[name] = `Вкажіть ${name === 'from' ? 'відправника' : name === 'to' ? 'отримувача' : 'вміст'}.`;
  const deliveryType = Number(raw.deliveryType);
  if (![1,2,3].includes(deliveryType)) errors.deliveryType = 'Оберіть тип доставки.';
  const parseNumber = value => Number(value.trim().replace(',', '.'));
  const weight = parseNumber(raw.weight);
  if (!raw.weight.trim() || !Number.isFinite(weight) || weight <= 0) errors.weight = 'Вага має бути більшою за 0.';
  const dimensions = {};
  for (const name of ['width','height','depth']) {
    dimensions[name] = parseNumber(raw[name]);
    if (!raw[name].trim() || !Number.isSafeInteger(dimensions[name]) || dimensions[name] <= 0) errors[name] = 'Вкажіть ціле число більше за 0.';
  }
  const value = raw.value.trim() ? parseNumber(raw.value) : null;
  if (value !== null && (!Number.isFinite(value) || value < 0)) errors.value = 'Вкажіть число не менше за 0.';
  return {errors, data:{from:raw.from.trim(),to:raw.to.trim(),content:raw.content.trim(),deliveryType,weight,width:dimensions.width,height:dimensions.height,depth:dimensions.depth,value,description:raw.description.trim() || null}};
}

async function saveForm(event, id) {
  event.preventDefault();
  if (state.busy) return;
  const form = event.currentTarget;
  const {errors,data} = readAndValidate(form);
  form.querySelectorAll('.field-error').forEach(element => element.textContent = '');
  form.querySelectorAll('.invalid').forEach(element => element.classList.remove('invalid'));
  const names = Object.keys(errors);
  if (names.length) {
    app.querySelector('#form-error').textContent = 'Виправте помилки у формі.';
    for (const name of names) { app.querySelector(`#error-${name}`).textContent = errors[name]; form.elements[name].classList.add('invalid'); }
    form.elements[names[0]].focus();
    return;
  }
  app.querySelector('#form-error').textContent = '';
  state.busy = true;
  const button = app.querySelector('#save-button');
  button.disabled = true; button.textContent = 'Збереження…';
  try {
    await api(id === null ? '/postings' : `/postings/${id}`, {method:id === null ? 'POST' : 'PUT',headers:{'Content-Type':'application/json'},body:JSON.stringify(id === null ? data : {id,...data})});
    toast(id === null ? 'Відправлення створено.' : 'Зміни збережено.');
    if (location.hash === '#/postings') route(); else location.hash = '#/postings';
  } catch (error) { app.querySelector('#form-error').textContent = `Не вдалося зберегти: ${error.message}`; }
  finally { state.busy = false; if (button.isConnected) { button.disabled = false; button.textContent = 'Зберегти'; } }
}

document.getElementById('menu-toggle').addEventListener('click', () => {
  const nav = document.getElementById('nav');
  const open = nav.classList.toggle('open');
  document.getElementById('menu-toggle').setAttribute('aria-expanded', String(open));
});
window.addEventListener('hashchange', route);
route();

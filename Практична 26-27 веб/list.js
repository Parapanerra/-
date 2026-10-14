const apiUrl = 'http://localhost:5211/postings';
const typeNames = {1:'Відділення',2:'Кур’єр',3:'Експрес-кур’єр'};
const weight = value => new Intl.NumberFormat('uk-UA',{maximumFractionDigits:2}).format(value);
const money = value => new Intl.NumberFormat('uk-UA',{minimumFractionDigits:2,maximumFractionDigits:2}).format(value);
const status = document.getElementById('status');
const tbody = document.getElementById('postings');
const cards = document.getElementById('cards');

function cell(text) {
  const td = document.createElement('td');
  td.textContent = text;
  return td;
}

function editLink(id) {
  const link = document.createElement('a');
  link.className = 'button primary';
  link.href = `edit.html?id=${encodeURIComponent(id)}`;
  link.textContent = 'Редагувати';
  return link;
}

function render(postings) {
  tbody.replaceChildren();
  cards.replaceChildren();
  for (const posting of postings) {
    const row = document.createElement('tr');
    for (const text of [posting.id, posting.from, posting.to,
      typeNames[posting.deliveryType] || 'Невідомо', weight(posting.weight), money(posting.price)]) {
      row.appendChild(cell(text));
    }
    const action = cell('');
    action.appendChild(editLink(posting.id));
    row.appendChild(action);
    tbody.appendChild(row);

    const card = document.createElement('article');
    card.className = 'card';
    const title = document.createElement('h2');
    title.textContent = `ID ${posting.id}`;
    card.appendChild(title);
    for (const text of [`Від: ${posting.from}`, `Кому: ${posting.to}`,
      `${typeNames[posting.deliveryType] || 'Невідомо'} / ${weight(posting.weight)} кг`,
      `${money(posting.price)} грн`]) {
      const paragraph = document.createElement('p');
      paragraph.textContent = text;
      card.appendChild(paragraph);
    }
    card.appendChild(editLink(posting.id));
    cards.appendChild(card);
  }
}

async function load() {
  try {
    const response = await fetch(apiUrl);
    if (!response.ok) throw new Error(`HTTP ${response.status}`);
    const postings = await response.json();
    render(postings);
    status.textContent = postings.length
      ? `Готово. Відправлень: ${postings.length}`
      : 'Відправлень немає.';
  } catch (error) {
    status.classList.add('error');
    status.textContent = `Не вдалося отримати список: ${error.message}`;
  }
}

document.getElementById('menu-toggle').addEventListener('click', () => {
  const nav = document.getElementById('nav');
  const open = nav.classList.toggle('open');
  document.getElementById('menu-toggle').setAttribute('aria-expanded', String(open));
});
load();

const form = document.getElementById('edit-form');
const statusBox = document.getElementById('page-status');
const formError = document.getElementById('form-error');
const saveButton = document.getElementById('save-button');
const cancelButton = document.getElementById('cancel-button');
const inputs = [...form.querySelectorAll('input, select, textarea')];
const editable = inputs.filter(input => !input.readOnly);
const id = Number(new URLSearchParams(location.search).get('id'));
const apiBase = location.port === '5211' ? '' : 'http://localhost:5211';
const maxFloat = 3.4028234663852886e38;
let saving = false;

const numeric = text => Number(text.trim().replace(',', '.'));
const messageFor = response => response.status === 404
  ? 'Відправлення не знайдено. Можливо, його вже видалили.'
  : `Сервер повернув помилку ${response.status}.`;

function validateField(input) {
  const value = input.value.trim();
  switch (input.name) {
    case 'from': return value ? '' : 'Вкажіть відправника.';
    case 'to': return value ? '' : 'Вкажіть отримувача.';
    case 'content': return value ? '' : 'Вкажіть вміст.';
    case 'deliveryType': return ['1', '2', '3'].includes(value) ? '' : 'Оберіть тип доставки.';
    case 'weight': {
      const weight = numeric(value);
      return value && Number.isFinite(weight) && weight > 0 && weight <= maxFloat
        ? '' : 'Вага має бути більшою за 0.';
    }
    case 'width':
    case 'height':
    case 'depth': {
      const size = numeric(value);
      return value && Number.isInteger(size) && size > 0 && size <= 2147483647
        ? '' : 'Вкажіть ціле число більше за 0.';
    }
    case 'value': {
      const declared = numeric(value);
      return !value || (Number.isFinite(declared) && declared >= 0 && declared <= maxFloat)
        ? '' : 'Вкажіть число не менше за 0.';
    }
    default: return '';
  }
}

function showError(input, message) {
  const output = document.getElementById(`error-${input.name}`);
  if (output) output.textContent = message;
  input.classList.toggle('invalid', Boolean(message));
  if (message) input.setAttribute('aria-invalid', 'true');
  else input.removeAttribute('aria-invalid');
}

function clearError(input) {
  showError(input, '');
  formError.textContent = '';
}

function validateAll() {
  let firstInvalid = null;
  for (const input of editable) {
    const message = validateField(input);
    showError(input, message);
    if (message && !firstInvalid) firstInvalid = input;
  }
  if (firstInvalid) {
    formError.textContent = 'Виправте помилки у формі.';
    firstInvalid.focus();
    return false;
  }
  formError.textContent = '';
  return true;
}

function formData() {
  return {
    id,
    from: form.elements.from.value.trim(),
    to: form.elements.to.value.trim(),
    content: form.elements.content.value.trim(),
    deliveryType: Number(form.elements.deliveryType.value),
    weight: numeric(form.elements.weight.value),
    width: numeric(form.elements.width.value),
    height: numeric(form.elements.height.value),
    depth: numeric(form.elements.depth.value),
    value: form.elements.value.value.trim() ? numeric(form.elements.value.value) : null,
    description: form.elements.description.value.trim() || null
  };
}

async function readError(response) {
  try {
    const body = await response.json();
    if (body.errors) return Object.values(body.errors).flat().join(' ');
    return body.detail || body.title || messageFor(response);
  } catch { return messageFor(response); }
}

async function loadPosting() {
  if (!Number.isSafeInteger(id) || id <= 0) {
    statusBox.classList.add('error');
    statusBox.textContent = 'Не вказано правильний ID відправлення. Поверніться до списку й оберіть запис.';
    return;
  }
  try {
    const response = await fetch(`${apiBase}/postings/${id}`);
    if (!response.ok) throw new Error(await readError(response));
    const posting = await response.json();
    for (const name of ['from', 'to', 'content', 'deliveryType', 'weight', 'width', 'height', 'depth', 'value', 'description']) {
      form.elements[name].value = posting[name] ?? '';
    }
    form.elements.price.value = new Intl.NumberFormat('uk-UA', {
      minimumFractionDigits: 2, maximumFractionDigits: 2
    }).format(posting.price);
    document.getElementById('posting-id').textContent = `ID ${id}`;
    statusBox.textContent = '';
    form.hidden = false;
  } catch (error) {
    statusBox.classList.add('error');
    statusBox.textContent = `Не вдалося завантажити відправлення: ${error.message}`;
  }
}

async function save() {
  if (saving || !validateAll()) return;
  saving = true;
  saveButton.disabled = true;
  saveButton.textContent = 'Збереження…';
  statusBox.classList.remove('error');
  statusBox.textContent = '';
  try {
    const response = await fetch(`${apiBase}/postings/${id}`, {
      method: 'PUT',
      headers: {'Content-Type': 'application/json'},
      body: JSON.stringify(formData())
    });
    if (!response.ok) throw new Error(await readError(response));
    location.assign('index.html');
  } catch (error) {
    statusBox.classList.add('error');
    statusBox.textContent = `Не вдалося зберегти зміни: ${error.message}`;
  } finally {
    saving = false;
    saveButton.disabled = false;
    saveButton.textContent = 'Зберегти';
  }
}

for (const input of inputs) {
  input.addEventListener('change', () => showError(input, validateField(input)));
  input.addEventListener('input', () => clearError(input));
}
saveButton.addEventListener('click', save);
cancelButton.addEventListener('click', () => location.assign('index.html'));
form.addEventListener('submit', event => { event.preventDefault(); save(); });
document.getElementById('menu-toggle').addEventListener('click', () => {
  const nav = document.getElementById('nav');
  const open = nav.classList.toggle('open');
  document.getElementById('menu-toggle').setAttribute('aria-expanded', String(open));
});
loadPosting();

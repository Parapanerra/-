using System.Globalization;
using System.Net;
using PostService.CommonTypes;
using PostService.Dtos;

namespace PostService.App;

public sealed class PostServiceWindow : Form
{
    private static readonly Color Ink = Color.FromArgb(38, 50, 70);
    private static readonly Color Muted = Color.FromArgb(93, 108, 128);
    private static readonly Color Surface = Color.FromArgb(245, 245, 245);
    private static readonly Color Border = Color.FromArgb(199, 208, 219);
    private static readonly Color Blue = Color.FromArgb(28, 96, 185);
    private static readonly Color Error = Color.FromArgb(183, 35, 48);
    private readonly IPostServiceClient _postService;
    private readonly Panel _page = new() { Dock = DockStyle.Fill, BackColor = Surface, AutoScroll = true };
    private readonly MenuStrip _menu = new();
    private PostingGetDto[] _postings = [];
    private DataGridView? _grid;
    private Label? _status;
    private Button? _editButton;
    private Button? _detailsButton;
    private Button? _deleteButton;
    private bool _busy;

    public PostServiceWindow(IPostServiceClient postService)
    {
        _postService = postService;
        Text = "PostService";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(1440, 850);
        MinimumSize = new Size(900, 650);
        BackColor = Surface;
        Font = new Font("Segoe UI", 12);
        ForeColor = Ink;

        _menu.BackColor = Color.FromArgb(240, 240, 240);
        _menu.ForeColor = Ink;
        _menu.Font = new Font("Segoe UI", 12);
        _menu.Height = 44;
        _menu.Items.Add("Головна", null, (_, _) => ShowHome());
        _menu.Items.Add("Відправлення", null, async (_, _) => await ShowListAsync());
        _menu.Items.Add("Про програму", null, (_, _) => ShowAbout());
        MainMenuStrip = _menu;
        Controls.Add(_page);
        Controls.Add(_menu);
        ShowHome();
    }

    private void BeginPage(string title)
    {
        _page.SuspendLayout();
        _page.Controls.Clear();
        _page.AutoScrollMinSize = new Size(1000, 710);
        Text = title;
        _page.ResumeLayout();
    }

    private static Label Label(string text, int x, int y, int width, int height = 38, float size = 13, bool bold = false, Color? color = null)
        => new()
        {
            Text = text, Location = new Point(x, y), Size = new Size(width, height),
            Font = new Font("Segoe UI", size, bold ? FontStyle.Bold : FontStyle.Regular),
            ForeColor = color ?? Ink, AutoEllipsis = false
        };

    private static Button Button(string text, int x, int y, int width, EventHandler onClick, bool primary = false)
    {
        var button = new Button
        {
            Text = text, Location = new Point(x, y), Size = new Size(width, 44),
            FlatStyle = FlatStyle.Flat, BackColor = primary ? Blue : Color.FromArgb(235, 239, 243),
            ForeColor = primary ? Color.White : Ink, Cursor = Cursors.Hand
        };
        button.FlatAppearance.BorderColor = primary ? Blue : Border;
        button.Click += onClick;
        return button;
    }

    private void ShowHome()
    {
        BeginPage("PostService");
        _page.Controls.Add(Label("Керування поштовими відправленнями", 48, 115, 850, 55, 25, true));
        _page.Controls.Add(Label("Оберіть розділ для роботи.", 48, 200, 650, 45, 17, color: Muted));
        _page.Controls.Add(Button("Список відправлень", 48, 300, 320, async (_, _) => await ShowListAsync()));
        _page.Controls.Add(Button("Про програму", 390, 300, 245, (_, _) => ShowAbout()));
    }

    private void ShowAbout()
    {
        BeginPage("Про програму");
        _page.Controls.Add(Label("PostService", 48, 75, 800, 70, 31, true, Blue));
        _page.Controls.Add(Label("Клієнтський застосунок для роботи з поштовими відправленнями.", 48, 185, 1100, 50, 18));
        _page.Controls.Add(Label("Перегляд, створення, редагування та видалення записів.\nДані зберігає сервер PostService через REST API.", 48, 295, 1100, 95, 18));
        _page.Controls.Add(Label("Навчальний проєкт. Настільний інтерфейс Windows Forms.", 48, 445, 1000, 50, 16, color: Muted));
        _page.Controls.Add(Button("На головну", 48, 545, 240, (_, _) => ShowHome()));
    }

    private async Task ShowListAsync()
    {
        BeginPage("Список поштових відправлень");
        _page.Controls.Add(Button("Нове відправлення", 48, 52, 260, async (_, _) => await ShowFormAsync()));
        _editButton = Button("Редагувати", 325, 52, 185, async (_, _) => await ShowFormAsync(SelectedPosting?.Id));
        _detailsButton = Button("Деталі", 527, 52, 150, async (_, _) => await ShowDetailsAsync(SelectedPosting?.Id));
        _deleteButton = Button("Видалити", 694, 52, 160, async (_, _) => await DeleteSelectedAsync());
        var refresh = Button("Оновити", 1160, 52, 190, async (_, _) => await RefreshAsync());
        refresh.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        _page.Controls.AddRange([_editButton, _detailsButton, _deleteButton, refresh]);

        _grid = new DataGridView
        {
            Location = new Point(48, 145), Size = new Size(1302, 485),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            BackgroundColor = Color.White, BorderStyle = BorderStyle.FixedSingle,
            RowHeadersVisible = false, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false, ReadOnly = true, MultiSelect = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            ColumnHeadersHeight = 62, RowTemplate = { Height = 57 },
            EnableHeadersVisualStyles = false, GridColor = Border
        };
        _grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(227, 234, 241);
        _grid.ColumnHeadersDefaultCellStyle.ForeColor = Ink;
        _grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 12, FontStyle.Bold);
        _grid.DefaultCellStyle.Font = new Font("Segoe UI", 12);
        _grid.DefaultCellStyle.ForeColor = Ink;
        _grid.DefaultCellStyle.SelectionBackColor = Blue;
        _grid.DefaultCellStyle.SelectionForeColor = Color.White;
        _grid.Columns.Add("Id", "ID");
        _grid.Columns.Add("From", "Відправник");
        _grid.Columns.Add("To", "Отримувач");
        _grid.Columns.Add("DeliveryType", "Тип доставки");
        _grid.Columns.Add("Weight", "Вага, кг");
        _grid.Columns.Add("Price", "Вартість, грн");
        _grid.Columns[0].FillWeight = 35;
        _grid.Columns[4].FillWeight = 55;
        _grid.Columns[5].FillWeight = 80;
        _grid.SelectionChanged += (_, _) => UpdateSelectionButtons();
        _grid.CellDoubleClick += async (_, _) => await ShowDetailsAsync(SelectedPosting?.Id);
        _page.Controls.Add(_grid);
        _status = Label("Завантаження відправлень…", 48, 695, 1100, 34, 13, color: Muted);
        _status.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        _page.Controls.Add(_status);
        UpdateSelectionButtons();
        await RefreshAsync();
    }

    private PostingGetDto? SelectedPosting => _grid?.SelectedRows.Count > 0
        ? _grid.SelectedRows[0].Tag as PostingGetDto : null;

    private void UpdateSelectionButtons()
    {
        var enabled = !_busy && SelectedPosting is not null;
        if (_editButton is not null) _editButton.Enabled = enabled;
        if (_detailsButton is not null) _detailsButton.Enabled = enabled;
        if (_deleteButton is not null) _deleteButton.Enabled = enabled;
    }

    private async Task RefreshAsync()
    {
        if (_grid is null || _status is null || _busy) return;
        SetBusy(true);
        _status.Text = "Завантаження відправлень…";
        UpdateSelectionButtons();
        try
        {
            if (!await TryRequestAsync(async () => _postings = await _postService.GetPostingsAsync()))
            {
                _status.Text = "Оновлення скасовано. Можна повторити пізніше.";
                return;
            }
            _grid.Rows.Clear();
            foreach (var posting in _postings)
            {
                var index = _grid.Rows.Add(posting.Id, posting.From, posting.To,
                    DeliveryName(posting.DeliveryType), Number(posting.Weight), Money(posting.Price));
                _grid.Rows[index].Tag = posting;
            }
            _grid.ClearSelection();
            _status.ForeColor = Muted;
            _status.Text = _postings.Length == 0 ? "Відправлень немає." : $"Готово. Відправлень: {_postings.Length}";
        }
        finally { SetBusy(false); UpdateSelectionButtons(); }
    }

    private async Task DeleteSelectedAsync()
    {
        var posting = SelectedPosting;
        if (posting is null || _busy) return;
        var choice = MessageBox.Show($"Видалити відправлення з ID {posting.Id}?\nВідновити запис після видалення неможливо.",
            "Підтвердження видалення", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
        if (choice != DialogResult.Yes) return;
        SetBusy(true);
        UpdateSelectionButtons();
        try
        {
            if (!await TryRequestAsync(() => _postService.DeletePostingAsync(posting.Id), true)) return;
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            MessageBox.Show("Вибране відправлення вже відсутнє. Список буде оновлено.", "Запис відсутній");
        }
        finally { SetBusy(false); }
        await RefreshAsync();
    }

    private async Task ShowDetailsAsync(int? id)
    {
        if (id is null) return;
        PostingGetDto? loaded = null;
        if (!await LoadPostingAsync(id.Value, posting => loaded = posting)) return;
        var posting = loaded!;
        BeginPage("Деталі відправлення");
        var entries = new (string, string)[]
        {
            ("ID", posting.Id.ToString()), ("Відправник", posting.From), ("Отримувач", posting.To),
            ("Вміст", posting.Content), ("Тип доставки", DeliveryName(posting.DeliveryType)),
            ("Вага, кг", Number(posting.Weight)), ("Ширина, см", posting.Width.ToString()),
            ("Висота, см", posting.Height.ToString()), ("Глибина, см", posting.Depth.ToString()),
            ("Оголошена цінність, грн", posting.Value is null ? "—" : Money(posting.Value.Value)),
            ("Вартість, грн", Money(posting.Price)), ("Опис", posting.Description ?? "—"),
            ("Створено (UTC)", posting.CreatedAt.ToUniversalTime().ToString("dd.MM.yyyy HH:mm", CultureInfo.GetCultureInfo("uk-UA")))
        };
        for (var i = 0; i < entries.Length; i++)
        {
            var y = 52 + i * 40;
            _page.Controls.Add(Label(entries[i].Item1, 48, y, 420, 35, 13, color: Muted));
            _page.Controls.Add(Label(entries[i].Item2, 480, y, 800, 35, 13));
        }
        _page.Controls.Add(Button("Назад до списку", 48, 625, 270, async (_, _) => await ShowListAsync()));
    }

    private async Task ShowFormAsync(int? id = null)
    {
        PostingGetDto? current = null;
        if (id is not null)
        {
            if (!await LoadPostingAsync(id.Value, posting => current = posting)) return;
        }
        BeginPage(id is null ? "Нове відправлення" : "Редагування відправлення");
        _page.AutoScrollMinSize = new Size(1160, 780);
        _page.Controls.Add(Label("Поля зі знаком * обов’язкові.", 48, 45, 800, 34, 13, color: Muted));
        var summary = Label("", 48, 80, 1050, 33, 13, true, Error);
        _page.Controls.Add(summary);
        var controls = new Dictionary<string, Control>();
        var errors = new Dictionary<string, Label>();
        var items = new (string key, string title, string value)[]
        {
            ("from", "Відправник *", current?.From ?? ""),
            ("to", "Отримувач *", current?.To ?? ""),
            ("content", "Вміст *", current?.Content ?? ""),
            ("deliveryType", "Тип доставки *", current is null ? "" : ((int)current.DeliveryType).ToString()),
            ("weight", "Вага, кг *", current is null ? "" : Number(current.Weight)),
            ("width", "Ширина, см *", current?.Width.ToString() ?? ""),
            ("height", "Висота, см *", current?.Height.ToString() ?? ""),
            ("depth", "Глибина, см *", current?.Depth.ToString() ?? ""),
            ("value", "Оголошена цінність, грн", current?.Value is float declaredAmount ? Number(declaredAmount) : ""),
            ("description", "Опис", current?.Description ?? "")
        };
        for (var i = 0; i < items.Length; i++)
        {
            var (key, title, value) = items[i];
            var y = 125 + i * 47;
            _page.Controls.Add(Label(title, 48, y + 4, 320, 34, 13));
            Control input;
            if (key == "deliveryType")
            {
                var combo = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList, Location = new Point(390, y),
                    Size = new Size(525, 36), Font = new Font("Segoe UI", 12)
                };
                combo.Items.AddRange(["Оберіть тип доставки", "Відділення", "Кур’єр", "Експрес-кур’єр"]);
                combo.SelectedIndex = int.TryParse(value, out var type) && type is >= 1 and <= 3 ? type : 0;
                input = combo;
            }
            else input = new TextBox
            {
                Text = value, Location = new Point(390, y), Size = new Size(525, 36),
                Font = new Font("Segoe UI", 12), BorderStyle = BorderStyle.FixedSingle
            };
            controls[key] = input;
            _page.Controls.Add(input);
            var error = Label("", 934, y + 4, 350, 36, 11, color: Error);
            errors[key] = error;
            _page.Controls.Add(error);
        }
        _page.Controls.Add(Label("Вартість, грн", 48, 598, 320, 34, 13));
        var price = new TextBox
        {
            Text = current is null ? "Обчислить сервер після збереження" : Money(current.Price),
            Location = new Point(390, 594), Size = new Size(525, 36), ReadOnly = true,
            BackColor = Color.FromArgb(231, 235, 240), Font = new Font("Segoe UI", 12)
        };
        _page.Controls.Add(price);
        Button? save = null;
        save = Button("Зберегти", 390, 675, 210, async (_, _) =>
        {
            if (_busy) return;
            summary.Text = "";
            foreach (var error in errors.Values) error.Text = "";
            foreach (var input in controls.Values) if (input is TextBox box) box.BackColor = Color.White;
            var from = controls["from"].Text.Trim();
            var to = controls["to"].Text.Trim();
            var content = controls["content"].Text.Trim();
            var description = controls["description"].Text.Trim();
            var deliveryType = ((ComboBox)controls["deliveryType"]).SelectedIndex;
            var invalid = new List<string>();
            void Fail(string key, string message)
            {
                errors[key].Text = "! " + message;
                if (controls[key] is TextBox box) box.BackColor = Color.MistyRose;
                invalid.Add(key);
            }
            if (from.Length == 0) Fail("from", "Вкажіть відправника.");
            if (to.Length == 0) Fail("to", "Вкажіть отримувача.");
            if (content.Length == 0) Fail("content", "Вкажіть вміст.");
            if (deliveryType is < 1 or > 3) Fail("deliveryType", "Оберіть тип доставки.");
            if (!TryFloat(controls["weight"].Text, out var weight) || weight <= 0) Fail("weight", "Вага має бути більшою за 0.");
            bool ParseDimension(string key, out int parsed)
            {
                if (int.TryParse(controls[key].Text.Trim(), out parsed) && parsed > 0) return true;
                Fail(key, "Вкажіть ціле число > 0.");
                return false;
            }
            ParseDimension("width", out var width);
            ParseDimension("height", out var height);
            ParseDimension("depth", out var depth);
            float? declaredValue = null;
            if (!string.IsNullOrWhiteSpace(controls["value"].Text))
            {
                if (!TryFloat(controls["value"].Text, out var parsedValue) || parsedValue < 0)
                    Fail("value", "Вкажіть число ≥ 0.");
                else declaredValue = parsedValue;
            }
            if (invalid.Count > 0)
            {
                summary.Text = "Виправте помилки у формі.";
                controls[invalid[0]].Focus();
                return;
            }
            SetBusy(true);
            save!.Enabled = false;
            var showList = false;
            try
            {
                var succeeded = await TryRequestAsync(async () =>
                {
                    _ = current is null
                    ? await _postService.CreatePostingAsync(new PostingPostDto
                    {
                        From = from, To = to, Content = content, DeliveryType = (DeliveryType)deliveryType,
                        Weight = weight, Width = width, Height = height, Depth = depth,
                        Value = declaredValue, Description = description.Length == 0 ? null : description
                    })
                    : await _postService.UpdatePostingAsync(current.Id, new PostingPutDto
                    {
                        Id = current.Id, From = from, To = to, Content = content,
                        DeliveryType = (DeliveryType)deliveryType, Weight = weight,
                        Width = width, Height = height, Depth = depth, Value = declaredValue,
                        Description = description.Length == 0 ? null : description
                    });
                }, true);
                if (!succeeded) { summary.Text = "Збереження скасовано. Дані форми залишено."; return; }
                showList = true;
            }
            catch (PostingValidationException ex)
            {
                summary.Text = "Сервер відхилив дані. Виправте помилки.";
                foreach (var field in ex.Errors)
                {
                    var key = errors.Keys.FirstOrDefault(key => string.Equals(key, field.Key, StringComparison.OrdinalIgnoreCase));
                    if (key is not null) errors[key].Text = "! " + string.Join(" ", field.Value);
                }
                MessageBox.Show(string.Join(Environment.NewLine, ex.Errors.SelectMany(field => field.Value)),
                    "Перевірте введені дані", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
            {
                MessageBox.Show("Відправлення вже видалено на сервері. Список буде оновлено.", "Запис відсутній");
                showList = true;
            }
            finally { SetBusy(false); if (!save!.IsDisposed) save.Enabled = true; }
            if (showList) await ShowListAsync();
        });
        _page.Controls.Add(save);
        _page.Controls.Add(Button("Скасувати", 620, 675, 210, async (_, _) => await ShowListAsync()));
    }

    private async Task<bool> LoadPostingAsync(int id, Action<PostingGetDto> receive)
    {
        SetBusy(true);
        var missing = false;
        try
        {
            return await TryRequestAsync(async () => receive(await _postService.GetPostingByIdAsync(id)));
        }
        catch (HttpRequestException ex) when (ex.StatusCode == HttpStatusCode.NotFound)
        {
            MessageBox.Show("Відправлення вже відсутнє. Список буде оновлено.", "Запис відсутній");
            missing = true;
        }
        finally { SetBusy(false); }
        if (missing) await ShowListAsync();
        return false;
    }

    private async Task<bool> TryRequestAsync(Func<Task> operation, bool changesData = false)
    {
        while (true)
        {
            try { await operation(); return true; }
            catch (HttpRequestException ex) when (ex.StatusCode is null or HttpStatusCode.ServiceUnavailable)
            {
                var warning = changesData
                    ? "\nЯкщо відповідь втрачено, попередня операція могла виконатися. Повторення створення може додати дублікат."
                    : "";
                if (MessageBox.Show("Сервер недоступний або не відповів вчасно. Повторити операцію?" + warning,
                    "Немає з’єднання", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning) != DialogResult.Retry)
                    return false;
            }
        }
    }

    private void SetBusy(bool value)
    {
        _busy = value;
        _menu.Enabled = !value;
        foreach (Control control in _page.Controls)
            if (control is Button) control.Enabled = !value;
        UseWaitCursor = value;
        UpdateSelectionButtons();
    }

    private static bool TryFloat(string text, out float value)
        => float.TryParse(text.Trim().Replace(',', '.'), NumberStyles.Float,
            CultureInfo.InvariantCulture, out value) && float.IsFinite(value);

    private static string Number(float value) => value.ToString("0.##", CultureInfo.GetCultureInfo("uk-UA"));
    private static string Money(float value) => value.ToString("0.00", CultureInfo.GetCultureInfo("uk-UA"));
    private static string DeliveryName(DeliveryType type) => type switch
    {
        DeliveryType.Department => "Відділення", DeliveryType.Courier => "Кур’єр",
        DeliveryType.ExpressCourier => "Експрес-кур’єр", _ => "Невідомо"
    };

}

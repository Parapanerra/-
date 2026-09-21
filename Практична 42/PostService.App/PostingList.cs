using PostService.Dtos;
using System.Net.Http.Json;

namespace PostService.App;

public partial class PostingList : Form
{
    private readonly HttpClient _httpClient = new()
    {
        BaseAddress = new Uri("http://localhost:5211/")
    };

    public PostingList()
    {
        InitializeComponent();
        gridPostings.AutoGenerateColumns = false;
    }

    private async void PostingList_Load(object? sender, EventArgs e)
    {
        await LoadPostingsAsync();
    }

    private async void btnRefresh_Click(object? sender, EventArgs e)
    {
        await LoadPostingsAsync();
    }

    private async void btnDelete_Click(object? sender, EventArgs e)
    {
        if (gridPostings.CurrentRow?.DataBoundItem is not PostingGetDto selectedPosting)
        {
            MessageBox.Show("Виберіть відправлення, яке потрібно видалити.", "Немає вибору",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        // Удаляем запись без подтверждения

        ToggleControls(false);
        try
        {
            using var response = await _httpClient.DeleteAsync($"postings/{selectedPosting.Id}");
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show($"Не вдалося видалити відправлення. Сервер повернув статус {(int)response.StatusCode}.",
                    "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (HttpRequestException)
        {
            ShowConnectionError();
        }
        finally
        {
            ToggleControls(true);
        }
    }

    private async Task LoadPostingsAsync()
    {
        ToggleControls(false);

        try
        {
            PostingGetDto[]? postings = null;
            HttpRequestException? lastError = null;

            for (var attempt = 0; attempt < 12; attempt++)
            {
                try
                {
                    postings = await _httpClient.GetFromJsonAsync<PostingGetDto[]>("postings");
                    break;
                }
                catch (HttpRequestException error) when (attempt < 11)
                {
                    lastError = error;
                    await Task.Delay(250);
                }
            }

            if (postings is null && lastError is not null) throw lastError;
            gridPostings.DataSource = postings ?? Array.Empty<PostingGetDto>();
        }
        catch (HttpRequestException)
        {
            gridPostings.DataSource = Array.Empty<PostingGetDto>();
            ShowConnectionError();
        }
        finally
        {
            ToggleControls(true);
        }
    }

    private void ToggleControls(bool enabled)
    {
        btnRefresh.Enabled = enabled;
        btnDelete.Enabled = enabled;
        UseWaitCursor = !enabled;
    }

    private static void ShowConnectionError() => MessageBox.Show(
        "Не вдалося отримати дані з сервера. Перевірте, чи запущено PostService на http://localhost:5211.",
        "Немає з'єднання", MessageBoxButtons.OK, MessageBoxIcon.Error);
}

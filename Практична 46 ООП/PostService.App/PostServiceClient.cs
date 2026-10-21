using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using PostService.Dtos;

namespace PostService.App;

public sealed class PostServiceClient : IPostServiceClient, IDisposable
{
    private readonly HttpClient _http;
    private readonly bool _ownsClient;

    public PostServiceClient(Uri? address = null)
    {
        _http = new HttpClient { BaseAddress = address ?? new Uri("http://localhost:8046/"), Timeout = TimeSpan.FromSeconds(10) };
        _ownsClient = true;
    }

    // Injected HttpClient is owned by the caller; useful for isolated contract checks.
    public PostServiceClient(HttpClient http) => _http = http;

    public Task<PostingGetDto[]> GetPostingsAsync() => ReadAsync<PostingGetDto[]>(() => _http.GetAsync("postings"));
    public Task<PostingGetDto> GetPostingByIdAsync(int id) => ReadAsync<PostingGetDto>(() => _http.GetAsync($"postings/{id}"));
    public Task<PostingGetDto> CreatePostingAsync(PostingPostDto posting) => ReadAsync<PostingGetDto>(() => _http.PostAsJsonAsync("postings", posting));
    public Task<PostingGetDto> UpdatePostingAsync(int id, PostingPutDto posting) => ReadAsync<PostingGetDto>(() => _http.PutAsJsonAsync($"postings/{id}", posting));

    public async Task DeletePostingAsync(int id)
    {
        using var response = await SendAsync(() => _http.DeleteAsync($"postings/{id}"));
        await EnsureSuccessAsync(response);
    }

    private static async Task<T> ReadAsync<T>(Func<Task<HttpResponseMessage>> send)
    {
        using var response = await SendAsync(send);
        await EnsureSuccessAsync(response);
        return await response.Content.ReadFromJsonAsync<T>()
            ?? throw new JsonException("Сервер повернув null замість очікуваних даних.");
    }

    private static async Task<HttpResponseMessage> SendAsync(Func<Task<HttpResponseMessage>> send)
    {
        try { return await send(); }
        catch (OperationCanceledException ex)
        {
            throw new HttpRequestException("Вичерпано час очікування відповіді сервера.", ex);
        }
    }

    private static async Task EnsureSuccessAsync(HttpResponseMessage response)
    {
        if (response.IsSuccessStatusCode) return;
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            // The existing API returns either ValidationProblem JSON or a JSON string for mismatched IDs.
            using var json = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
            if (json.RootElement.ValueKind == JsonValueKind.String)
                throw new PostingValidationException(new() { ["request"] = [json.RootElement.GetString() ?? "Некоректний запит."] });
            var problem = json.RootElement.Deserialize<ValidationErrorResponse>(JsonSerializerOptions.Web)
                ?? throw new JsonException("Порожня відповідь перевірки даних.");
            throw new PostingValidationException(problem.Errors is { Count: > 0 } errors
                ? errors : new() { ["request"] = [problem.Title ?? "Перевірте введені дані."] });
        }
        throw new HttpRequestException($"Сервер повернув помилку {(int)response.StatusCode}.", null, response.StatusCode);
    }

    public void Dispose()
    {
        if (_ownsClient) _http.Dispose();
    }
}

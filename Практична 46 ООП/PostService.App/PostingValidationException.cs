namespace PostService.App;

public sealed class PostingValidationException : Exception
{
    public IReadOnlyDictionary<string, string[]> Errors { get; }

    public PostingValidationException(Dictionary<string, string[]> errors)
        : base("Сервер відхилив дані відправлення.")
        => Errors = errors.ToDictionary(pair => pair.Key, pair => pair.Value.ToArray(), StringComparer.OrdinalIgnoreCase);
}

internal sealed class ValidationErrorResponse
{
    public Dictionary<string, string[]>? Errors { get; set; }
    public string? Title { get; set; }
}

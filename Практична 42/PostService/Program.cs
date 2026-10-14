using PostService.BusinessLogic;
using PostService.Dtos;
using PostService.Mappings;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors(options => options.AddDefaultPolicy(policy =>
    policy.WithOrigins("http://localhost:3000", "http://localhost:60648")
        .AllowAnyHeader()
        .AllowAnyMethod()));

builder.Services.AddScoped<IPostingRepository, PostingRepository>();
builder.Services.AddScoped<IPostingService, PostingService>();

var app = builder.Build();
app.UseCors();
app.UseDefaultFiles();
app.UseStaticFiles();

using (var scope = app.Services.CreateScope())
{
    var repository = scope.ServiceProvider.GetRequiredService<IPostingRepository>();
    repository.CreateDb();
}


app.MapGet("/postings", (IPostingService postingService) =>
{
    var postings = postingService.GetAll().Select(PostingMapper.ToPostingGetDto).ToList();
    return Results.Ok(postings);
});

app.MapGet("/postings/{id}", (int id, IPostingService postingService) =>
{
    var posting = postingService.Find(id);
    return posting == null ? Results.NotFound() : Results.Ok(PostingMapper.ToPostingGetDto(posting));
});

app.MapPost("/postings", (PostingPostDto postDto, IPostingService postingService) =>
{
    var errors = ValidatePosting(postDto.From, postDto.To, postDto.Content,
        (int)postDto.DeliveryType, postDto.Weight, postDto.Width, postDto.Height,
        postDto.Depth, postDto.Value);
    if (errors.Count > 0) return Results.ValidationProblem(errors);

    var savedPosting = postingService.Create(PostingMapper.ToPosting(postDto));
    var resultDto = PostingMapper.ToPostingGetDto(savedPosting);
    return Results.Created($"/postings/{resultDto.Id}", resultDto);
});

app.MapPut("/postings/{id}", (int id, PostingPutDto putDto, IPostingService postingService) =>
{
    if (id != putDto.Id)
    {
        return Results.BadRequest("Id у маршруті не збігається з Id у тілі запиту.");
    }

    var errors = ValidatePosting(putDto.From, putDto.To, putDto.Content,
        (int)putDto.DeliveryType, putDto.Weight, putDto.Width, putDto.Height,
        putDto.Depth, putDto.Value);
    if (errors.Count > 0) return Results.ValidationProblem(errors);

    var updatedPosting = postingService.Update(PostingMapper.ToPosting(putDto));
    return updatedPosting == null ? Results.NotFound() : Results.Ok(PostingMapper.ToPostingGetDto(updatedPosting));
});

app.MapDelete("/postings/{id}", (int id, IPostingService postingService) =>
    postingService.Delete(id) == 0 ? Results.NotFound() : Results.NoContent());

app.Run();

static Dictionary<string, string[]> ValidatePosting(string from, string to, string content,
    int deliveryType, float weight, int width, int height, int depth, float? value)
{
    var errors = new Dictionary<string, string[]>();
    if (string.IsNullOrWhiteSpace(from)) errors[nameof(from)] = ["Вкажіть відправника."];
    if (string.IsNullOrWhiteSpace(to)) errors[nameof(to)] = ["Вкажіть отримувача."];
    if (string.IsNullOrWhiteSpace(content)) errors[nameof(content)] = ["Вкажіть вміст."];
    if (deliveryType is < 1 or > 3) errors[nameof(deliveryType)] = ["Оберіть тип доставки."];
    if (!float.IsFinite(weight) || weight <= 0) errors[nameof(weight)] = ["Вага має бути більшою за 0."];
    if (width <= 0) errors[nameof(width)] = ["Ширина має бути більшою за 0."];
    if (height <= 0) errors[nameof(height)] = ["Висота має бути більшою за 0."];
    if (depth <= 0) errors[nameof(depth)] = ["Глибина має бути більшою за 0."];
    if (value is float declaredValue && (!float.IsFinite(declaredValue) || declaredValue < 0))
        errors[nameof(value)] = ["Оголошена цінність має бути не меншою за 0."];
    return errors;
}

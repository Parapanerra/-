using Microsoft.OpenApi.Models;
using PostService.Documentation;
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

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PostService REST API",
        Version = "v1",
        Description = "Облік поштових відправлень. Створення, перегляд, редагування та видалення записів. Усі тіла запитів і відповіді передаються у JSON."
    });
    foreach (var assembly in new[] { typeof(Program).Assembly, typeof(PostingGetDto).Assembly, typeof(PostService.CommonTypes.DeliveryType).Assembly })
        options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, assembly.GetName().Name + ".xml"));
    options.SupportNonNullableReferenceTypes();
    options.UseAllOfToExtendReferenceSchemas();
    options.OperationFilter<PostingOperationFilter>();
});

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "PostService v1");
    options.DocumentTitle = "PostService Swagger документація";
});
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
})
.WithTags("Поштові відправлення")
.WithSummary("Отримати список відправлень")
.WithDescription("Повертає всі збережені відправлення. Вхідні параметри відсутні. Якщо записів немає, повертає порожній масив.")
.Produces<List<PostingGetDto>>(200);

app.MapGet("/postings/{id}", (int id, IPostingService postingService) =>
{
    var posting = postingService.Find(id);
    return posting == null ? Results.NotFound() : Results.Ok(PostingMapper.ToPostingGetDto(posting));
})
.WithTags("Поштові відправлення")
.WithSummary("Отримати відправлення за ID")
.WithDescription("Повертає повні дані одного відправлення за ідентифікатором у маршруті. Якщо запис відсутній, повертає 404.")
.Produces<PostingGetDto>(200).Produces(404).Produces(400);

app.MapPost("/postings", (PostingPostDto postDto, IPostingService postingService) =>
{
    var errors = ValidatePosting(postDto.From, postDto.To, postDto.Content,
        (int)postDto.DeliveryType, postDto.Weight, postDto.Width, postDto.Height,
        postDto.Depth, postDto.Value);
    if (errors.Count > 0) return Results.ValidationProblem(errors);

    var savedPosting = postingService.Create(PostingMapper.ToPosting(postDto));
    var resultDto = PostingMapper.ToPostingGetDto(savedPosting);
    return Results.Created($"/postings/{resultDto.Id}", resultDto);
})
.WithTags("Поштові відправлення")
.WithSummary("Створити відправлення")
.WithDescription("Приймає JSON PostingPostDto. Перевіряє обов’язкові поля, тип доставки, вагу, розміри та оголошену цінність. Сервер призначає ID, дату та обчислює вартість. Відповідь 201 містить створений запис і заголовок Location.")
.Accepts<PostingPostDto>("application/json").Produces<PostingGetDto>(201).ProducesValidationProblem();

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
})
.WithTags("Поштові відправлення")
.WithSummary("Оновити відправлення")
.WithDescription("Замінює дані наявного відправлення значеннями PostingPutDto. ID у JSON має збігатися з id у маршруті. Виконуються перевірки введених даних. Повертає оновлений запис або 404.")
.Accepts<PostingPutDto>("application/json").Produces<PostingGetDto>(200).Produces(404).ProducesValidationProblem();

app.MapDelete("/postings/{id}", (int id, IPostingService postingService) =>
    postingService.Delete(id) == 0 ? Results.NotFound() : Results.NoContent())
.WithTags("Поштові відправлення")
.WithSummary("Видалити відправлення")
.WithDescription("Видаляє відправлення за ідентифікатором у маршруті. Успішна відповідь 204 не містить тіла. Якщо запис відсутній, повертає 404.")
.Produces(204).Produces(404).Produces(400);

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

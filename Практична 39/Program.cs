using PostService.Dtos;
using PostService.Mappings;
using PostService.Models;
using PostService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPostingService, PostingService>();

var app = builder.Build();

app.MapGet("/", () => "PostService API is running");

app.MapGet("/postings", (IPostingService postingService) =>
{
    var postings = postingService.GetAll()
        .Select(PostingMapper.ToPostingGetDto)
        .ToList();

    return Results.Ok(postings);
});

app.MapGet("/postings/{id}", (int id, IPostingService postingService) =>
{
    var posting = postingService.Find(id);

    if (posting == null)
    {
        return Results.NotFound();
    }

    var resultDto = PostingMapper.ToPostingGetDto(posting);
    return Results.Ok(resultDto);
});

app.MapPost("/postings", (PostingPostDto postDto, IPostingService postingService) =>
{
    Posting newPosting = PostingMapper.ToPosting(postDto);
    var savedObject = postingService.Create(newPosting);
    var resultDto = PostingMapper.ToPostingGetDto(savedObject);

    return Results.Created($"/postings/{resultDto.Id}", resultDto);
});

app.MapPut("/postings/{id}", (int id, PostingPutDto putDto, IPostingService postingService) =>
{
    if (id != putDto.Id)
    {
        return Results.BadRequest("Id у маршруті не збігається з Id у тілі запиту.");
    }

    Posting posting = PostingMapper.ToPosting(putDto);
    var updatedPosting = postingService.Update(posting);

    if (updatedPosting == null)
    {
        return Results.NotFound();
    }

    var resultDto = PostingMapper.ToPostingGetDto(updatedPosting);
    return Results.Ok(resultDto);
});

app.MapDelete("/postings/{id}", (int id, IPostingService postingService) =>
{
    int deletedCount = postingService.Delete(id);

    if (deletedCount == 0)
    {
        return Results.NotFound();
    }

    return Results.NoContent();
});

app.Run();

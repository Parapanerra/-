using PostService.BusinessLogic;
using PostService.Dtos;
using PostService.Mappings;
using PostService.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPostingService, PostingService>();

var app = builder.Build();

app.MapGet("/", () => "PostService API is running");

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

	var updatedPosting = postingService.Update(PostingMapper.ToPosting(putDto));
	return updatedPosting == null ? Results.NotFound() : Results.Ok(PostingMapper.ToPostingGetDto(updatedPosting));
});

app.MapDelete("/postings/{id}", (int id, IPostingService postingService) =>
	postingService.Delete(id) == 0 ? Results.NotFound() : Results.NoContent());

app.Run();

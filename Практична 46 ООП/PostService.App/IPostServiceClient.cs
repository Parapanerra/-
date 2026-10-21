using PostService.Dtos;

namespace PostService.App;

public interface IPostServiceClient
{
    Task<PostingGetDto[]> GetPostingsAsync();
    Task<PostingGetDto> GetPostingByIdAsync(int id);
    Task<PostingGetDto> CreatePostingAsync(PostingPostDto posting);
    Task<PostingGetDto> UpdatePostingAsync(int id, PostingPutDto posting);
    Task DeletePostingAsync(int id);
}

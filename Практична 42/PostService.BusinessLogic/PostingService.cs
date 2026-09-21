using PostService.CommonTypes;
using PostService.Models;

namespace PostService.BusinessLogic;

public class PostingService : IPostingService
{
    private readonly IPostingRepository _repository;

    public PostingService(IPostingRepository repository)
    {
        _repository = repository;
    }

    public Posting Create(Posting newPosting)
    {
        newPosting.CreatedAt = DateTime.UtcNow;
        newPosting.Price = CalculatePrice(newPosting.Weight, newPosting.DeliveryType);

        var postingId = _repository.Create(newPosting);
        var savedPosting = (Posting)newPosting.Clone();
        savedPosting.Id = postingId;
        return savedPosting;
    }

    public List<Posting> GetAll() => _repository.GetList();

    public Posting? Find(int postingId) => _repository.GetById(postingId);

    public Posting? Update(Posting posting)
    {
        var existingPosting = _repository.GetById(posting.Id);
        if (existingPosting == null)
        {
            return null;
        }

        existingPosting.From = posting.From;
        existingPosting.To = posting.To;
        existingPosting.Content = posting.Content;
        existingPosting.DeliveryType = posting.DeliveryType;
        existingPosting.Weight = posting.Weight;
        existingPosting.Width = posting.Width;
        existingPosting.Height = posting.Height;
        existingPosting.Depth = posting.Depth;
        existingPosting.Value = posting.Value;
        existingPosting.Description = posting.Description;
        existingPosting.Price = CalculatePrice(posting.Weight, posting.DeliveryType);

        _repository.Update(existingPosting);
        return existingPosting;
    }

    public int Delete(int postingId) => _repository.Delete(postingId);

    private static float CalculatePrice(float weight, DeliveryType deliveryType)
    {
        float basePrice = deliveryType switch
        {
            DeliveryType.Department => 50,
            DeliveryType.Courier => 80,
            DeliveryType.ExpressCourier => 120,
            _ => 50
        };

        return basePrice + weight * 12;
    }
}

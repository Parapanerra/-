using PostService.Models;

namespace PostService.Services;

public class PostingService : IPostingService
{
    private static readonly List<Posting> _postings = new();

    public Posting Create(Posting newPosting)
    {
        int maxId = 1;

        if (_postings.Count > 0)
        {
            maxId = _postings.Max(posting => posting.Id) + 1;
        }

        newPosting.Id = maxId;
        newPosting.CreatedAt = DateTime.UtcNow;
        newPosting.Price = CalculatePrice(newPosting.Weight, newPosting.DeliveryType);

        _postings.Add(newPosting);

        return newPosting;
    }

    public List<Posting> GetAll()
    {
        return _postings;
    }

    public Posting? Find(int postingId)
    {
        return _postings.FirstOrDefault(posting => posting.Id == postingId);
    }

    public Posting? Update(Posting posting)
    {
        var existingPosting = Find(posting.Id);

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
        existingPosting.Price = CalculatePrice(posting.Weight, posting.DeliveryType);

        return existingPosting;
    }

    public int Delete(int postingId)
    {
        var posting = Find(postingId);

        if (posting == null)
        {
            return 0;
        }

        _postings.Remove(posting);
        return 1;
    }

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

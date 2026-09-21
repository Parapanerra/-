using PostService.CommonTypes;

namespace PostService.Models;

public class Posting
{
    public int Id { get; set; }
    public string From { get; set; } = string.Empty;
    public string To { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public DeliveryType DeliveryType { get; set; }
    public float Weight { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public int Depth { get; set; }
    public float? Value { get; set; }
    public float Price { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

    public object Clone()
    {
        return new Posting
        {
            Id = Id,
            From = From,
            To = To,
            Content = Content,
            DeliveryType = DeliveryType,
            Weight = Weight,
            Width = Width,
            Height = Height,
            Depth = Depth,
            Value = Value,
            Price = Price,
            Description = Description,
            CreatedAt = CreatedAt
        };
    }
}

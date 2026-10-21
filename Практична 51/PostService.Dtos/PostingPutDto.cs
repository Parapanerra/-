using PostService.CommonTypes;

namespace PostService.Dtos;

/// <summary>Повний набір даних для оновлення відправлення.</summary>
public class PostingPutDto
{
    /// <summary>Унікальний ідентифікатор відправлення. Під час оновлення має збігатися з id у маршруті.</summary>
    public int Id { get; set; }
    /// <summary>Відправник. Обов’язковий непорожній рядок.</summary>
    public string From { get; set; } = string.Empty;
    /// <summary>Отримувач. Обов’язковий непорожній рядок.</summary>
    public string To { get; set; } = string.Empty;
    /// <summary>Вміст відправлення. Обов’язковий непорожній рядок.</summary>
    public string Content { get; set; } = string.Empty;
    /// <summary>Тип доставки: 1 — відділення, 2 — кур’єр, 3 — експрес-кур’єр.</summary>
    public DeliveryType DeliveryType { get; set; }
    /// <summary>Вага в кілограмах. Скінченне число більше за 0.</summary>
    public float Weight { get; set; }
    /// <summary>Ширина в сантиметрах. Ціле число більше за 0.</summary>
    public int Width { get; set; }
    /// <summary>Висота в сантиметрах. Ціле число більше за 0.</summary>
    public int Height { get; set; }
    /// <summary>Глибина в сантиметрах. Ціле число більше за 0.</summary>
    public int Depth { get; set; }
    /// <summary>Оголошена цінність у гривнях. Необов’язкове скінченне число не менше за 0; допускається null.</summary>
    public float? Value { get; set; }
    /// <summary>Додатковий опис. Необов’язковий рядок; допускається null.</summary>
    public string? Description { get; set; }
}

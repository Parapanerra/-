namespace PostService.CommonTypes;

/// <summary>Спосіб доставки: 1 — відділення, 2 — кур’єр, 3 — експрес-кур’єр.</summary>
public enum DeliveryType
{
    /// <summary>Доставка у відділення.</summary>
    Department = 1,
    /// <summary>Кур’єрська доставка.</summary>
    Courier = 2,
    /// <summary>Експрес-кур’єрська доставка.</summary>
    ExpressCourier = 3
}

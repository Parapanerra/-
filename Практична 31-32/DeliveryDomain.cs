namespace WindowsFormsApp31_32;

public class VolumeWeight
{
    public decimal Weight { get; set; }
    public decimal Length { get; set; }
    public decimal Width { get; set; }
    public decimal Height { get; set; }

    public decimal VolumetricWeight
    {
        get
        {
            return Length * Width * Height / 4000m;
        }
    }

    public decimal ChargeableWeight
    {
        get
        {
            return Math.Max(Weight, VolumetricWeight);
        }
    }
}

public enum DeliveryType
{
    Department,
    Courier,
    ExpressCourier
}

public interface IDeliveryCostStrategy
{
    DeliveryType DeliveryType { get; }
    decimal Calculate(VolumeWeight volumeWeight);
}

public class DepartmentDeliveryStrategy : IDeliveryCostStrategy
{
    public DeliveryType DeliveryType => DeliveryType.Department;

    public decimal Calculate(VolumeWeight volumeWeight)
    {
        return 40m + volumeWeight.ChargeableWeight * 10m;
    }
}

public class CourierDeliveryStrategy : IDeliveryCostStrategy
{
    public DeliveryType DeliveryType => DeliveryType.Courier;

    public decimal Calculate(VolumeWeight volumeWeight)
    {
        return 80m + volumeWeight.ChargeableWeight * 15m;
    }
}

public class ExpressCourierDeliveryStrategy : IDeliveryCostStrategy
{
    public DeliveryType DeliveryType => DeliveryType.ExpressCourier;

    public decimal Calculate(VolumeWeight volumeWeight)
    {
        return 120m + volumeWeight.ChargeableWeight * 24m;
    }
}

public interface IDeliveryCostService
{
    decimal Calculate(VolumeWeight volumeWeight, DeliveryType deliveryType);
}

public class EarlyBindingDeliveryCostService : IDeliveryCostService
{
    public decimal Calculate(VolumeWeight volumeWeight, DeliveryType deliveryType)
    {
        return deliveryType switch
        {
            DeliveryType.Department =>
                new DepartmentDeliveryStrategy().Calculate(volumeWeight),
            DeliveryType.Courier =>
                new CourierDeliveryStrategy().Calculate(volumeWeight),
            DeliveryType.ExpressCourier =>
                new ExpressCourierDeliveryStrategy().Calculate(volumeWeight),
            _ => throw new ArgumentException("Невідомий спосіб доставки.")
        };
    }
}

public class DeliveryCostService : IDeliveryCostService
{
    private readonly List<IDeliveryCostStrategy> strategies;

    public DeliveryCostService(IEnumerable<IDeliveryCostStrategy> strategies)
    {
        this.strategies = strategies.ToList();
    }

    public decimal Calculate(VolumeWeight volumeWeight, DeliveryType deliveryType)
    {
        IDeliveryCostStrategy? strategy = strategies.FirstOrDefault(
            item => item.DeliveryType == deliveryType);

        if (strategy == null)
        {
            throw new ArgumentException("Невідомий спосіб доставки.");
        }

        return strategy.Calculate(volumeWeight);
    }
}

public class DeliveryTypeItem
{
    public DeliveryType Type { get; }
    public string Name { get; }

    public DeliveryTypeItem(DeliveryType type, string name)
    {
        Type = type;
        Name = name;
    }

    public override string ToString()
    {
        return Name;
    }
}

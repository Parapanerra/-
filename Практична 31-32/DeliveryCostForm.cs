namespace WindowsFormsApp31_32;

public partial class DeliveryCostForm : Form
{
    private readonly IDeliveryCostService deliveryCostService;

    public DeliveryCostForm()
    {
        InitializeComponent();

        deliveryCostService = new DeliveryCostService(new IDeliveryCostStrategy[]
        {
            new DepartmentDeliveryStrategy(),
            new CourierDeliveryStrategy(),
            new ExpressCourierDeliveryStrategy()
        });

        InitializeDeliveryTypes();
        ResetResults();
    }

    private void InitializeDeliveryTypes()
    {
        deliveryTypeComboBox.Items.Add(new DeliveryTypeItem(
            DeliveryType.Department,
            "Доставка у відділення"));
        deliveryTypeComboBox.Items.Add(new DeliveryTypeItem(
            DeliveryType.Courier,
            "Кур'єром додому"));
        deliveryTypeComboBox.Items.Add(new DeliveryTypeItem(
            DeliveryType.ExpressCourier,
            "Експрес кур'єром додому"));
        deliveryTypeComboBox.SelectedIndex = 0;
    }

    private void calculateButton_Click(object sender, EventArgs e)
    {
        try
        {
            VolumeWeight volumeWeight = new VolumeWeight
            {
                Weight = weightNumericUpDown.Value,
                Length = lengthNumericUpDown.Value,
                Width = widthNumericUpDown.Value,
                Height = heightNumericUpDown.Value
            };

            DeliveryType deliveryType = GetSelectedDeliveryType();
            decimal deliveryCost = deliveryCostService.Calculate(
                volumeWeight,
                deliveryType);

            ShowResults(volumeWeight, deliveryType, deliveryCost);
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                ex.Message,
                "Помилка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
        }
    }

    private void clearButton_Click(object sender, EventArgs e)
    {
        weightNumericUpDown.Value = 0;
        lengthNumericUpDown.Value = 0;
        widthNumericUpDown.Value = 0;
        heightNumericUpDown.Value = 0;
        deliveryTypeComboBox.SelectedIndex = 0;
        ResetResults();
        statusLabel.Text = "Готово до розрахунку";
    }

    private DeliveryType GetSelectedDeliveryType()
    {
        if (deliveryTypeComboBox.SelectedItem is DeliveryTypeItem item)
        {
            return item.Type;
        }

        throw new InvalidOperationException("Оберіть спосіб доставки.");
    }

    private string GetDeliveryName(DeliveryType deliveryType)
    {
        return deliveryType switch
        {
            DeliveryType.Department => "У відділення",
            DeliveryType.Courier => "Кур'єром додому",
            DeliveryType.ExpressCourier => "Експрес кур'єром",
            _ => "Невідомо"
        };
    }

    private void ShowResults(
        VolumeWeight volumeWeight,
        DeliveryType deliveryType,
        decimal deliveryCost)
    {
        actualWeightValueLabel.Text = $"{volumeWeight.Weight:0.##} кг";
        volumetricWeightValueLabel.Text = $"{volumeWeight.VolumetricWeight:0.##} кг";
        chargeableWeightValueLabel.Text = $"{volumeWeight.ChargeableWeight:0.##} кг";
        deliveryTypeValueLabel.Text = GetDeliveryName(deliveryType);
        totalCostValueLabel.Text = $"{deliveryCost:0.##} грн";
        statusLabel.Text = "Розрахунок виконано";
    }

    private void ResetResults()
    {
        actualWeightValueLabel.Text = "-";
        volumetricWeightValueLabel.Text = "-";
        chargeableWeightValueLabel.Text = "-";
        deliveryTypeValueLabel.Text = "-";
        totalCostValueLabel.Text = "0 грн";
    }
}

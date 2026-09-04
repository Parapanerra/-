#nullable disable

namespace WindowsFormsApp31_32;

partial class DeliveryCostForm
{
    private System.ComponentModel.IContainer components = null;
    private GroupBox parcelGroupBox;
    private Label weightLabel;
    private Label lengthLabel;
    private Label widthLabel;
    private Label heightLabel;
    private Label formulaLabel;
    private NumericUpDown weightNumericUpDown;
    private NumericUpDown lengthNumericUpDown;
    private NumericUpDown widthNumericUpDown;
    private NumericUpDown heightNumericUpDown;
    private GroupBox deliveryGroupBox;
    private ComboBox deliveryTypeComboBox;
    private Button calculateButton;
    private Button clearButton;
    private GroupBox resultGroupBox;
    private Label actualWeightLabel;
    private Label volumetricWeightLabel;
    private Label chargeableWeightLabel;
    private Label deliveryTypeLabel;
    private Label actualWeightValueLabel;
    private Label volumetricWeightValueLabel;
    private Label chargeableWeightValueLabel;
    private Label deliveryTypeValueLabel;
    private Label totalCostTitleLabel;
    private Label totalCostValueLabel;
    private StatusStrip statusStrip;
    private ToolStripStatusLabel statusLabel;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
        {
            components.Dispose();
        }

        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        parcelGroupBox = new GroupBox();
        weightLabel = new Label();
        lengthLabel = new Label();
        widthLabel = new Label();
        heightLabel = new Label();
        formulaLabel = new Label();
        weightNumericUpDown = new NumericUpDown();
        lengthNumericUpDown = new NumericUpDown();
        widthNumericUpDown = new NumericUpDown();
        heightNumericUpDown = new NumericUpDown();
        deliveryGroupBox = new GroupBox();
        deliveryTypeComboBox = new ComboBox();
        calculateButton = new Button();
        clearButton = new Button();
        resultGroupBox = new GroupBox();
        actualWeightLabel = new Label();
        volumetricWeightLabel = new Label();
        chargeableWeightLabel = new Label();
        deliveryTypeLabel = new Label();
        actualWeightValueLabel = new Label();
        volumetricWeightValueLabel = new Label();
        chargeableWeightValueLabel = new Label();
        deliveryTypeValueLabel = new Label();
        totalCostTitleLabel = new Label();
        totalCostValueLabel = new Label();
        statusStrip = new StatusStrip();
        statusLabel = new ToolStripStatusLabel();
        parcelGroupBox.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)weightNumericUpDown).BeginInit();
        ((System.ComponentModel.ISupportInitialize)lengthNumericUpDown).BeginInit();
        ((System.ComponentModel.ISupportInitialize)widthNumericUpDown).BeginInit();
        ((System.ComponentModel.ISupportInitialize)heightNumericUpDown).BeginInit();
        deliveryGroupBox.SuspendLayout();
        resultGroupBox.SuspendLayout();
        statusStrip.SuspendLayout();
        SuspendLayout();

        parcelGroupBox.Controls.Add(weightLabel);
        parcelGroupBox.Controls.Add(lengthLabel);
        parcelGroupBox.Controls.Add(widthLabel);
        parcelGroupBox.Controls.Add(heightLabel);
        parcelGroupBox.Controls.Add(formulaLabel);
        parcelGroupBox.Controls.Add(weightNumericUpDown);
        parcelGroupBox.Controls.Add(lengthNumericUpDown);
        parcelGroupBox.Controls.Add(widthNumericUpDown);
        parcelGroupBox.Controls.Add(heightNumericUpDown);
        parcelGroupBox.Location = new Point(12, 12);
        parcelGroupBox.Name = "parcelGroupBox";
        parcelGroupBox.Size = new Size(310, 208);
        parcelGroupBox.TabIndex = 0;
        parcelGroupBox.TabStop = false;
        parcelGroupBox.Text = "Параметри посилки";

        weightLabel.AutoSize = true;
        weightLabel.Location = new Point(16, 37);
        weightLabel.Name = "weightLabel";
        weightLabel.Size = new Size(61, 20);
        weightLabel.Text = "Вага, кг";

        lengthLabel.AutoSize = true;
        lengthLabel.Location = new Point(16, 78);
        lengthLabel.Name = "lengthLabel";
        lengthLabel.Size = new Size(86, 20);
        lengthLabel.Text = "Довжина, см";

        widthLabel.AutoSize = true;
        widthLabel.Location = new Point(16, 119);
        widthLabel.Name = "widthLabel";
        widthLabel.Size = new Size(83, 20);
        widthLabel.Text = "Ширина, см";

        heightLabel.AutoSize = true;
        heightLabel.Location = new Point(16, 160);
        heightLabel.Name = "heightLabel";
        heightLabel.Size = new Size(80, 20);
        heightLabel.Text = "Висота, см";

        formulaLabel.AutoSize = true;
        formulaLabel.ForeColor = SystemColors.GrayText;
        formulaLabel.Location = new Point(16, 183);
        formulaLabel.Name = "formulaLabel";
        formulaLabel.Size = new Size(193, 20);
        formulaLabel.Text = "Об'ємна вага = (Д × Ш × В) / 4000";

        ConfigureNumberInput(weightNumericUpDown, "weightNumericUpDown", 140, 34, 2.5m);
        ConfigureNumberInput(lengthNumericUpDown, "lengthNumericUpDown", 140, 75, 40m);
        ConfigureNumberInput(widthNumericUpDown, "widthNumericUpDown", 140, 116, 30m);
        ConfigureNumberInput(heightNumericUpDown, "heightNumericUpDown", 140, 157, 20m);

        deliveryGroupBox.Controls.Add(deliveryTypeComboBox);
        deliveryGroupBox.Location = new Point(12, 231);
        deliveryGroupBox.Name = "deliveryGroupBox";
        deliveryGroupBox.Size = new Size(310, 84);
        deliveryGroupBox.TabIndex = 1;
        deliveryGroupBox.TabStop = false;
        deliveryGroupBox.Text = "Спосіб доставки";

        deliveryTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
        deliveryTypeComboBox.FormattingEnabled = true;
        deliveryTypeComboBox.Location = new Point(16, 34);
        deliveryTypeComboBox.Name = "deliveryTypeComboBox";
        deliveryTypeComboBox.Size = new Size(275, 28);
        deliveryTypeComboBox.TabIndex = 0;

        calculateButton.BackColor = Color.FromArgb(0, 120, 215);
        calculateButton.ForeColor = Color.White;
        calculateButton.Location = new Point(12, 331);
        calculateButton.Name = "calculateButton";
        calculateButton.Size = new Size(148, 40);
        calculateButton.TabIndex = 2;
        calculateButton.Text = "Розрахувати";
        calculateButton.UseVisualStyleBackColor = false;
        calculateButton.Click += calculateButton_Click;

        clearButton.Location = new Point(174, 331);
        clearButton.Name = "clearButton";
        clearButton.Size = new Size(148, 40);
        clearButton.TabIndex = 3;
        clearButton.Text = "Очистити";
        clearButton.UseVisualStyleBackColor = true;
        clearButton.Click += clearButton_Click;

        resultGroupBox.Controls.Add(actualWeightLabel);
        resultGroupBox.Controls.Add(volumetricWeightLabel);
        resultGroupBox.Controls.Add(chargeableWeightLabel);
        resultGroupBox.Controls.Add(deliveryTypeLabel);
        resultGroupBox.Controls.Add(actualWeightValueLabel);
        resultGroupBox.Controls.Add(volumetricWeightValueLabel);
        resultGroupBox.Controls.Add(chargeableWeightValueLabel);
        resultGroupBox.Controls.Add(deliveryTypeValueLabel);
        resultGroupBox.Controls.Add(totalCostTitleLabel);
        resultGroupBox.Controls.Add(totalCostValueLabel);
        resultGroupBox.Location = new Point(337, 12);
        resultGroupBox.Name = "resultGroupBox";
        resultGroupBox.Size = new Size(380, 380);
        resultGroupBox.TabIndex = 4;
        resultGroupBox.TabStop = false;
        resultGroupBox.Text = "Результат";

        ConfigureResultLabel(actualWeightLabel, "Фактична вага:", 24, 48);
        ConfigureResultLabel(volumetricWeightLabel, "Об'ємна вага:", 24, 102);
        ConfigureResultLabel(chargeableWeightLabel, "Розрахункова вага:", 24, 156);
        ConfigureResultLabel(deliveryTypeLabel, "Спосіб доставки:", 24, 210);
        ConfigureResultValue(actualWeightValueLabel, 235, 48);
        ConfigureResultValue(volumetricWeightValueLabel, 235, 102);
        ConfigureResultValue(chargeableWeightValueLabel, 235, 156);
        ConfigureResultValue(deliveryTypeValueLabel, 155, 204);
        deliveryTypeValueLabel.Size = new Size(205, 46);

        totalCostTitleLabel.BackColor = Color.FromArgb(232, 243, 255);
        totalCostTitleLabel.BorderStyle = BorderStyle.FixedSingle;
        totalCostTitleLabel.Font = new Font("Segoe UI", 17F, FontStyle.Bold);
        totalCostTitleLabel.ForeColor = Color.FromArgb(0, 86, 179);
        totalCostTitleLabel.Location = new Point(15, 270);
        totalCostTitleLabel.Name = "totalCostTitleLabel";
        totalCostTitleLabel.Size = new Size(350, 42);
        totalCostTitleLabel.Text = "Вартість доставки";
        totalCostTitleLabel.TextAlign = ContentAlignment.MiddleCenter;

        totalCostValueLabel.BackColor = Color.FromArgb(232, 243, 255);
        totalCostValueLabel.BorderStyle = BorderStyle.FixedSingle;
        totalCostValueLabel.Font = new Font("Segoe UI", 26F, FontStyle.Bold);
        totalCostValueLabel.ForeColor = Color.FromArgb(0, 86, 179);
        totalCostValueLabel.Location = new Point(15, 311);
        totalCostValueLabel.Name = "totalCostValueLabel";
        totalCostValueLabel.Size = new Size(350, 56);
        totalCostValueLabel.Text = "0 грн";
        totalCostValueLabel.TextAlign = ContentAlignment.MiddleCenter;

        statusStrip.Items.AddRange(new ToolStripItem[] { statusLabel });
        statusStrip.Location = new Point(0, 416);
        statusStrip.Name = "statusStrip";
        statusStrip.Size = new Size(729, 26);
        statusStrip.TabIndex = 5;

        statusLabel.Name = "statusLabel";
        statusLabel.Size = new Size(143, 20);
        statusLabel.Text = "Готово до розрахунку";

        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(729, 442);
        Controls.Add(resultGroupBox);
        Controls.Add(clearButton);
        Controls.Add(calculateButton);
        Controls.Add(deliveryGroupBox);
        Controls.Add(parcelGroupBox);
        Controls.Add(statusStrip);
        FormBorderStyle = FormBorderStyle.FixedSingle;
        MaximizeBox = false;
        Name = "DeliveryCostForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Розрахунок вартості доставки";
        parcelGroupBox.ResumeLayout(false);
        parcelGroupBox.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)weightNumericUpDown).EndInit();
        ((System.ComponentModel.ISupportInitialize)lengthNumericUpDown).EndInit();
        ((System.ComponentModel.ISupportInitialize)widthNumericUpDown).EndInit();
        ((System.ComponentModel.ISupportInitialize)heightNumericUpDown).EndInit();
        deliveryGroupBox.ResumeLayout(false);
        resultGroupBox.ResumeLayout(false);
        resultGroupBox.PerformLayout();
        statusStrip.ResumeLayout(false);
        statusStrip.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    private static void ConfigureNumberInput(
        NumericUpDown input,
        string name,
        int x,
        int y,
        decimal value)
    {
        input.DecimalPlaces = 2;
        input.Increment = 0.1m;
        input.Location = new Point(x, y);
        input.Maximum = 1000000m;
        input.Name = name;
        input.Size = new Size(151, 27);
        input.TabIndex = 0;
        input.Value = value;
    }

    private static void ConfigureResultLabel(
        Label label,
        string text,
        int x,
        int y)
    {
        label.AutoSize = true;
        label.Location = new Point(x, y);
        label.Name = "resultLabel";
        label.Size = new Size(100, 20);
        label.Text = text;
    }

    private static void ConfigureResultValue(Label label, int x, int y)
    {
        label.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
        label.Location = new Point(x, y);
        label.Name = "resultValueLabel";
        label.Size = new Size(125, 25);
        label.Text = "-";
        label.TextAlign = ContentAlignment.MiddleRight;
    }
}

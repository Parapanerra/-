#nullable disable

namespace WindowsFormsApp14;

partial class CalculatorForm
{
    private System.ComponentModel.IContainer components = null;
    private Panel pnlDisplay;
    private Label lblDisplay;
    private TableLayoutPanel tlpButtonsPanel;
    private Button btnClear;
    private Button btnBackspace;
    private Button btnPercent;
    private Button btnDivide;
    private Button btnSeven;
    private Button btnEight;
    private Button btnNine;
    private Button btnMultiply;
    private Button btnFour;
    private Button btnFive;
    private Button btnSix;
    private Button btnSubtract;
    private Button btnOne;
    private Button btnTwo;
    private Button btnThree;
    private Button btnAdd;
    private Button btnZero;
    private Button btnDecimal;
    private Button btnEquals;

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
        pnlDisplay = new Panel();
        lblDisplay = new Label();
        tlpButtonsPanel = new TableLayoutPanel();
        btnClear = CreateButton("btnClear", "CE");
        btnBackspace = CreateButton("btnBackspace", "←");
        btnPercent = CreateButton("btnPercent", "%");
        btnDivide = CreateButton("btnDivide", "/");
        btnSeven = CreateButton("btnSeven", "7");
        btnEight = CreateButton("btnEight", "8");
        btnNine = CreateButton("btnNine", "9");
        btnMultiply = CreateButton("btnMultiply", "*");
        btnFour = CreateButton("btnFour", "4");
        btnFive = CreateButton("btnFive", "5");
        btnSix = CreateButton("btnSix", "6");
        btnSubtract = CreateButton("btnSubtract", "-");
        btnOne = CreateButton("btnOne", "1");
        btnTwo = CreateButton("btnTwo", "2");
        btnThree = CreateButton("btnThree", "3");
        btnAdd = CreateButton("btnAdd", "+");
        btnZero = CreateButton("btnZero", "0");
        btnDecimal = CreateButton("btnDecimal", ".");
        btnEquals = CreateButton("btnEquals", "=");
        pnlDisplay.SuspendLayout();
        tlpButtonsPanel.SuspendLayout();
        SuspendLayout();

        pnlDisplay.BackColor = SystemColors.Control;
        pnlDisplay.Controls.Add(lblDisplay);
        pnlDisplay.Dock = DockStyle.Top;
        pnlDisplay.Name = "pnlDisplay";
        pnlDisplay.Padding = new Padding(5);
        pnlDisplay.Size = new Size(384, 80);

        lblDisplay.BackColor = Color.White;
        lblDisplay.Dock = DockStyle.Fill;
        lblDisplay.Font = new Font("Segoe UI", 24F);
        lblDisplay.Name = "lblDisplay";
        lblDisplay.Text = "0";
        lblDisplay.TextAlign = ContentAlignment.MiddleRight;

        tlpButtonsPanel.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        tlpButtonsPanel.ColumnCount = 4;
        tlpButtonsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tlpButtonsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tlpButtonsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tlpButtonsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tlpButtonsPanel.Controls.Add(btnClear, 0, 0);
        tlpButtonsPanel.Controls.Add(btnBackspace, 1, 0);
        tlpButtonsPanel.Controls.Add(btnPercent, 2, 0);
        tlpButtonsPanel.Controls.Add(btnDivide, 3, 0);
        tlpButtonsPanel.Controls.Add(btnSeven, 0, 1);
        tlpButtonsPanel.Controls.Add(btnEight, 1, 1);
        tlpButtonsPanel.Controls.Add(btnNine, 2, 1);
        tlpButtonsPanel.Controls.Add(btnMultiply, 3, 1);
        tlpButtonsPanel.Controls.Add(btnFour, 0, 2);
        tlpButtonsPanel.Controls.Add(btnFive, 1, 2);
        tlpButtonsPanel.Controls.Add(btnSix, 2, 2);
        tlpButtonsPanel.Controls.Add(btnSubtract, 3, 2);
        tlpButtonsPanel.Controls.Add(btnOne, 0, 3);
        tlpButtonsPanel.Controls.Add(btnTwo, 1, 3);
        tlpButtonsPanel.Controls.Add(btnThree, 2, 3);
        tlpButtonsPanel.Controls.Add(btnAdd, 3, 3);
        tlpButtonsPanel.Controls.Add(btnZero, 0, 4);
        tlpButtonsPanel.Controls.Add(btnDecimal, 1, 4);
        tlpButtonsPanel.Controls.Add(btnEquals, 2, 4);
        tlpButtonsPanel.Dock = DockStyle.Fill;
        tlpButtonsPanel.Name = "tlpButtonsPanel";
        tlpButtonsPanel.RowCount = 5;
        tlpButtonsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
        tlpButtonsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
        tlpButtonsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
        tlpButtonsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
        tlpButtonsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
        tlpButtonsPanel.SetColumnSpan(btnEquals, 2);

        btnClear.ForeColor = Color.OrangeRed;
        btnClear.Click += btnClear_Click;
        btnBackspace.Click += btnBackspace_Click;
        btnPercent.Click += btnPercent_Click;
        btnDivide.Click += btnDivide_Click;
        btnMultiply.Click += btnMultiply_Click;
        btnSubtract.Click += btnSubtract_Click;
        btnAdd.Click += btnAdd_Click;
        btnEquals.Click += btnEquals_Click;
        btnZero.Click += NumberButton_Click;
        btnOne.Click += NumberButton_Click;
        btnTwo.Click += NumberButton_Click;
        btnThree.Click += NumberButton_Click;
        btnFour.Click += NumberButton_Click;
        btnFive.Click += NumberButton_Click;
        btnSix.Click += NumberButton_Click;
        btnSeven.Click += NumberButton_Click;
        btnEight.Click += NumberButton_Click;
        btnNine.Click += NumberButton_Click;
        btnDecimal.Click += NumberButton_Click;

        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(384, 481);
        Controls.Add(tlpButtonsPanel);
        Controls.Add(pnlDisplay);
        MaximizeBox = false;
        MinimizeBox = false;
        MinimumSize = new Size(320, 420);
        Name = "CalculatorForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Калькулятор";
        pnlDisplay.ResumeLayout(false);
        tlpButtonsPanel.ResumeLayout(false);
        ResumeLayout(false);
    }

    private static Button CreateButton(string name, string text)
    {
        return new Button
        {
            Dock = DockStyle.Fill,
            Font = new Font("Segoe UI", 16F),
            Margin = new Padding(2),
            Name = name,
            Text = text,
            UseVisualStyleBackColor = true
        };
    }
}

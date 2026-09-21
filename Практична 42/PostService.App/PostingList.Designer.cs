#nullable enable

namespace PostService.App;

partial class PostingList
{
    private System.ComponentModel.IContainer? components = null;
    private DataGridView gridPostings = null!;
    private Button btnDelete = null!;
    private Button btnRefresh = null!;

    protected override void Dispose(bool disposing)
    {
        if (disposing) components?.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        gridPostings = new DataGridView();
        btnDelete = new Button();
        btnRefresh = new Button();
        var colId = new DataGridViewTextBoxColumn();
        var colFrom = new DataGridViewTextBoxColumn();
        var colTo = new DataGridViewTextBoxColumn();
        var colContent = new DataGridViewTextBoxColumn();
        var colDeliveryType = new DataGridViewTextBoxColumn();
        var colWeight = new DataGridViewTextBoxColumn();
        var colWidth = new DataGridViewTextBoxColumn();
        var colHeight = new DataGridViewTextBoxColumn();
        var colDepth = new DataGridViewTextBoxColumn();
        var colValue = new DataGridViewTextBoxColumn();
        var colPrice = new DataGridViewTextBoxColumn();
        var colCreatedAt = new DataGridViewTextBoxColumn();
        ((System.ComponentModel.ISupportInitialize)gridPostings).BeginInit();
        SuspendLayout();

        gridPostings.AllowUserToAddRows = false;
        gridPostings.AllowUserToDeleteRows = false;
        gridPostings.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        gridPostings.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        gridPostings.Columns.AddRange(new DataGridViewColumn[] { colId, colFrom, colTo, colContent, colDeliveryType, colWeight, colWidth, colHeight, colDepth, colValue, colPrice, colCreatedAt });
        gridPostings.Location = new Point(12, 12);
        gridPostings.MultiSelect = false;
        gridPostings.Name = "gridPostings";
        gridPostings.ReadOnly = true;
        gridPostings.RowHeadersWidth = 51;
        gridPostings.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        gridPostings.Size = new Size(960, 420);
        gridPostings.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;

        ConfigureColumn(colId, "Id", "Id", false);
        ConfigureColumn(colFrom, "Від кого", "From");
        ConfigureColumn(colTo, "Кому", "To");
        ConfigureColumn(colContent, "Вміст", "Content");
        ConfigureColumn(colDeliveryType, "Тип доставки", "DeliveryType");
        ConfigureColumn(colWeight, "Вага", "Weight");
        ConfigureColumn(colWidth, "Ширина", "Width");
        ConfigureColumn(colHeight, "Висота", "Height");
        ConfigureColumn(colDepth, "Глибина", "Depth");
        ConfigureColumn(colValue, "Оцінена вартість", "Value");
        ConfigureColumn(colPrice, "Ціна", "Price");
        ConfigureColumn(colCreatedAt, "Створено", "CreatedAt", false);

        btnDelete.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnDelete.Location = new Point(822, 450);
        btnDelete.Name = "btnDelete";
        btnDelete.Size = new Size(150, 35);
        btnDelete.Text = "Видалити пост";
        btnDelete.UseVisualStyleBackColor = true;
        btnDelete.Click += btnDelete_Click;

        btnRefresh.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        btnRefresh.Location = new Point(658, 450);
        btnRefresh.Name = "btnRefresh";
        btnRefresh.Size = new Size(150, 35);
        btnRefresh.Text = "Оновити дані";
        btnRefresh.UseVisualStyleBackColor = true;
        btnRefresh.Click += btnRefresh_Click;

        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(984, 501);
        Controls.Add(btnRefresh);
        Controls.Add(btnDelete);
        Controls.Add(gridPostings);
        MinimumSize = new Size(700, 350);
        Name = "PostingList";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Поштові відправлення";
        Load += PostingList_Load;
        ((System.ComponentModel.ISupportInitialize)gridPostings).EndInit();
        ResumeLayout(false);
    }

    private static void ConfigureColumn(DataGridViewTextBoxColumn column, string header, string property, bool visible = true)
    {
        column.DataPropertyName = property;
        column.HeaderText = header;
        column.Name = $"col{property}";
        column.Visible = visible;
    }
}

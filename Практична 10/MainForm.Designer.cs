#nullable disable

namespace WindowsFormsApp10;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;
    private Panel inputPanel = null!;
    private Label nameLabel = null!;
    private TextBox nameTextBox = null!;
    private Button greetButton = null!;

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
        inputPanel = new Panel();
        nameLabel = new Label();
        nameTextBox = new TextBox();
        greetButton = new Button();
        inputPanel.SuspendLayout();
        SuspendLayout();

        inputPanel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        inputPanel.BackColor = SystemColors.ControlDark;
        inputPanel.BorderStyle = BorderStyle.FixedSingle;
        inputPanel.Controls.Add(nameLabel);
        inputPanel.Controls.Add(nameTextBox);
        inputPanel.Location = new Point(20, 20);
        inputPanel.Name = "inputPanel";
        inputPanel.Size = new Size(544, 82);
        inputPanel.TabIndex = 0;

        nameLabel.Anchor = AnchorStyles.Left;
        nameLabel.AutoSize = true;
        nameLabel.Location = new Point(18, 29);
        nameLabel.Name = "nameLabel";
        nameLabel.Size = new Size(115, 20);
        nameLabel.TabIndex = 0;
        nameLabel.Text = "Введіть своє ім'я";

        nameTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
        nameTextBox.Location = new Point(151, 26);
        nameTextBox.Name = "nameTextBox";
        nameTextBox.Size = new Size(370, 27);
        nameTextBox.TabIndex = 1;

        greetButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        greetButton.Location = new Point(446, 120);
        greetButton.Name = "greetButton";
        greetButton.Size = new Size(118, 36);
        greetButton.TabIndex = 1;
        greetButton.Text = "Привітати";
        greetButton.UseVisualStyleBackColor = true;
        greetButton.Click += greetButton_Click;

        AcceptButton = greetButton;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(584, 181);
        Controls.Add(greetButton);
        Controls.Add(inputPanel);
        MinimumSize = new Size(430, 228);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Привітання";
        inputPanel.ResumeLayout(false);
        inputPanel.PerformLayout();
        ResumeLayout(false);
    }
}

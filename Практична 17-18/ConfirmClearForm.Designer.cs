#nullable disable

namespace WindowsFormsApp17_18;

partial class ConfirmClearForm
{
    private System.ComponentModel.IContainer components = null;
    private Label confirmationLabel;
    private Button buttonYes;
    private Button buttonNo;
    private FlowLayoutPanel buttonsPanel;

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
        confirmationLabel = new Label();
        buttonYes = new Button();
        buttonNo = new Button();
        buttonsPanel = new FlowLayoutPanel();
        buttonsPanel.SuspendLayout();
        SuspendLayout();

        confirmationLabel.Dock = DockStyle.Fill;
        confirmationLabel.Font = new Font("Segoe UI", 12F);
        confirmationLabel.ForeColor = Color.OrangeRed;
        confirmationLabel.Location = new Point(15, 15);
        confirmationLabel.Name = "confirmationLabel";
        confirmationLabel.Size = new Size(390, 62);
        confirmationLabel.TabIndex = 0;
        confirmationLabel.Text = "Ви дійсно хочете очистити вміст калькулятора?";
        confirmationLabel.TextAlign = ContentAlignment.MiddleCenter;

        buttonsPanel.Controls.Add(buttonYes);
        buttonsPanel.Controls.Add(buttonNo);
        buttonsPanel.Dock = DockStyle.Bottom;
        buttonsPanel.FlowDirection = FlowDirection.LeftToRight;
        buttonsPanel.Location = new Point(15, 77);
        buttonsPanel.Name = "buttonsPanel";
        buttonsPanel.Padding = new Padding(105, 8, 0, 0);
        buttonsPanel.Size = new Size(390, 54);
        buttonsPanel.TabIndex = 1;
        buttonsPanel.WrapContents = false;

        buttonYes.DialogResult = DialogResult.Yes;
        buttonYes.Location = new Point(108, 11);
        buttonYes.Name = "buttonYes";
        buttonYes.Size = new Size(80, 30);
        buttonYes.TabIndex = 0;
        buttonYes.Text = "Так";
        buttonYes.UseVisualStyleBackColor = true;
        buttonYes.Click += buttonYes_Click;

        buttonNo.DialogResult = DialogResult.No;
        buttonNo.Location = new Point(194, 11);
        buttonNo.Name = "buttonNo";
        buttonNo.Size = new Size(80, 30);
        buttonNo.TabIndex = 1;
        buttonNo.Text = "Ні";
        buttonNo.UseVisualStyleBackColor = true;
        buttonNo.Click += buttonNo_Click;

        AcceptButton = buttonYes;
        AutoScaleDimensions = new SizeF(8F, 20F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = buttonNo;
        ClientSize = new Size(420, 146);
        Controls.Add(confirmationLabel);
        Controls.Add(buttonsPanel);
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "ConfirmClearForm";
        Padding = new Padding(15);
        ShowInTaskbar = false;
        StartPosition = FormStartPosition.CenterParent;
        Text = "Очистити вміст калькулятора";
        buttonsPanel.ResumeLayout(false);
        ResumeLayout(false);
    }
}

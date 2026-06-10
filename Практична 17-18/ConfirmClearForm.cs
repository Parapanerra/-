namespace WindowsFormsApp17_18;

public partial class ConfirmClearForm : Form
{
    public ConfirmClearForm()
    {
        InitializeComponent();
    }

    private void buttonYes_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Yes;
        Close();
    }

    private void buttonNo_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.No;
        Close();
    }
}

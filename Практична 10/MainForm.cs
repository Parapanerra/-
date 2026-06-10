namespace WindowsFormsApp10;

public partial class MainForm : Form
{
    public MainForm()
    {
        InitializeComponent();
    }

    private void greetButton_Click(object sender, EventArgs e)
    {
        string name = nameTextBox.Text.Trim();

        if (name.Length == 0)
        {
            MessageBox.Show(
                "Будь ласка, введіть своє ім'я.",
                "Перевірка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning);
            return;
        }

        MessageBox.Show(
            $"Привіт, {name}!",
            "Привітання",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }
}

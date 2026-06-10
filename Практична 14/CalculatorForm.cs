using System.Globalization;

namespace WindowsFormsApp14;

public partial class CalculatorForm : Form
{
    private double firstNumber;
    private string operation = "";
    private bool isOperationSelected;

    public CalculatorForm()
    {
        InitializeComponent();
    }

    private void NumberButton_Click(object sender, EventArgs e)
    {
        string buttonText = ((Button)sender).Text;
        string decimalSeparator =
            CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        if (isOperationSelected)
        {
            lblDisplay.Text = "0";
            isOperationSelected = false;
        }

        if (buttonText == ".")
        {
            if (!lblDisplay.Text.Contains(decimalSeparator))
            {
                lblDisplay.Text += decimalSeparator;
            }

            return;
        }

        if (lblDisplay.Text == "0")
        {
            lblDisplay.Text = buttonText;
        }
        else
        {
            lblDisplay.Text += buttonText;
        }
    }

    private void btnAdd_Click(object sender, EventArgs e)
    {
        SelectOperation("+");
    }

    private void btnSubtract_Click(object sender, EventArgs e)
    {
        SelectOperation("-");
    }

    private void btnMultiply_Click(object sender, EventArgs e)
    {
        SelectOperation("*");
    }

    private void btnDivide_Click(object sender, EventArgs e)
    {
        SelectOperation("/");
    }

    private void SelectOperation(string selectedOperation)
    {
        firstNumber = ReadDisplay();
        operation = selectedOperation;
        isOperationSelected = true;
    }

    private void btnEquals_Click(object sender, EventArgs e)
    {
        double secondNumber = ReadDisplay();
        double result;

        switch (operation)
        {
            case "+":
                result = firstNumber + secondNumber;
                break;
            case "-":
                result = firstNumber - secondNumber;
                break;
            case "*":
                result = firstNumber * secondNumber;
                break;
            case "/":
                if (secondNumber == 0)
                {
                    MessageBox.Show(
                        "Ділення на нуль неможливе.",
                        "Помилка",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                    return;
                }

                result = firstNumber / secondNumber;
                break;
            default:
                return;
        }

        lblDisplay.Text = result.ToString(CultureInfo.CurrentCulture);
        operation = "";
        isOperationSelected = true;
    }

    private void btnClear_Click(object sender, EventArgs e)
    {
        firstNumber = 0;
        operation = "";
        isOperationSelected = false;
        lblDisplay.Text = "0";
    }

    private void btnBackspace_Click(object sender, EventArgs e)
    {
        if (lblDisplay.Text.Length > 1)
        {
            lblDisplay.Text = lblDisplay.Text[..^1];
        }
        else
        {
            lblDisplay.Text = "0";
        }
    }

    private void btnPercent_Click(object sender, EventArgs e)
    {
        double number = ReadDisplay();
        lblDisplay.Text = (number / 100).ToString(CultureInfo.CurrentCulture);
    }

    private double ReadDisplay()
    {
        return double.Parse(lblDisplay.Text, CultureInfo.CurrentCulture);
    }
}

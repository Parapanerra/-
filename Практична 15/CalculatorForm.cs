using System.Globalization;

namespace WindowsFormsApp15;

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
        AddInputSymbol(buttonText);
    }

    private void AddInputSymbol(string symbol)
    {
        string decimalSeparator =
            CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        if (isOperationSelected)
        {
            lblDisplay.Text = "0";
            isOperationSelected = false;
        }

        if (symbol == ".")
        {
            if (!lblDisplay.Text.Contains(decimalSeparator))
            {
                lblDisplay.Text += decimalSeparator;
            }

            return;
        }

        if (lblDisplay.Text == "0")
        {
            lblDisplay.Text = symbol;
        }
        else
        {
            lblDisplay.Text += symbol;
        }
    }

    private void CalculatorForm_KeyDown(object sender, KeyEventArgs e)
    {
        string? digit = e.KeyCode switch
        {
            Keys.D0 or Keys.NumPad0 => "0",
            Keys.D1 or Keys.NumPad1 => "1",
            Keys.D2 or Keys.NumPad2 => "2",
            Keys.D3 or Keys.NumPad3 => "3",
            Keys.D4 or Keys.NumPad4 => "4",
            Keys.D5 or Keys.NumPad5 => "5",
            Keys.D6 or Keys.NumPad6 => "6",
            Keys.D7 or Keys.NumPad7 => "7",
            Keys.D8 or Keys.NumPad8 => "8",
            Keys.D9 or Keys.NumPad9 => "9",
            _ => null
        };

        if (digit == null)
        {
            return;
        }

        AddInputSymbol(digit);
        e.Handled = true;
        e.SuppressKeyPress = true;
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

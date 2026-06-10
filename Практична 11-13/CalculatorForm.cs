using System.Globalization;

namespace WindowsFormsApp11_13;

public partial class CalculatorForm : Form
{
    private decimal firstValue;
    private string? currentOperation;
    private bool startNewNumber = true;

    public CalculatorForm()
    {
        InitializeComponent();
    }

    private void numberButton_Click(object sender, EventArgs e)
    {
        if (sender is not Button button)
        {
            return;
        }

        if (startNewNumber || lblDisplay.Text == "0")
        {
            lblDisplay.Text = button.Text;
            startNewNumber = false;
        }
        else
        {
            lblDisplay.Text += button.Text;
        }
    }

    private void decimalButton_Click(object sender, EventArgs e)
    {
        string separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

        if (startNewNumber)
        {
            lblDisplay.Text = "0" + separator;
            startNewNumber = false;
        }
        else if (!lblDisplay.Text.Contains(separator))
        {
            lblDisplay.Text += separator;
        }
    }

    private void operationButton_Click(object sender, EventArgs e)
    {
        if (sender is not Button button || !TryReadDisplay(out decimal value))
        {
            return;
        }

        if (currentOperation != null && !startNewNumber)
        {
            CalculateResult();
            TryReadDisplay(out value);
        }

        firstValue = value;
        currentOperation = button.Text;
        startNewNumber = true;
    }

    private void equalsButton_Click(object sender, EventArgs e)
    {
        CalculateResult();
    }

    private void clearButton_Click(object sender, EventArgs e)
    {
        firstValue = 0;
        currentOperation = null;
        startNewNumber = true;
        lblDisplay.Text = "0";
    }

    private void backspaceButton_Click(object sender, EventArgs e)
    {
        if (startNewNumber || lblDisplay.Text.Length <= 1)
        {
            lblDisplay.Text = "0";
            startNewNumber = true;
            return;
        }

        lblDisplay.Text = lblDisplay.Text[..^1];
    }

    private void percentButton_Click(object sender, EventArgs e)
    {
        if (TryReadDisplay(out decimal value))
        {
            ShowValue(value / 100);
            startNewNumber = true;
        }
    }

    private void CalculateResult()
    {
        if (currentOperation == null || !TryReadDisplay(out decimal secondValue))
        {
            return;
        }

        try
        {
            decimal result = currentOperation switch
            {
                "+" => firstValue + secondValue,
                "-" => firstValue - secondValue,
                "*" => firstValue * secondValue,
                "/" when secondValue != 0 => firstValue / secondValue,
                "/" => throw new DivideByZeroException(),
                _ => secondValue
            };

            ShowValue(result);
            firstValue = result;
            currentOperation = null;
            startNewNumber = true;
        }
        catch (DivideByZeroException)
        {
            MessageBox.Show(
                "Ділення на нуль неможливе.",
                "Помилка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            clearButton_Click(this, EventArgs.Empty);
        }
        catch (OverflowException)
        {
            MessageBox.Show(
                "Результат виходить за допустимий діапазон.",
                "Помилка",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            clearButton_Click(this, EventArgs.Empty);
        }
    }

    private bool TryReadDisplay(out decimal value)
    {
        return decimal.TryParse(
            lblDisplay.Text,
            NumberStyles.Number,
            CultureInfo.CurrentCulture,
            out value);
    }

    private void ShowValue(decimal value)
    {
        lblDisplay.Text = value.ToString("G29", CultureInfo.CurrentCulture);
    }
}

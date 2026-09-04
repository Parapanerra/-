namespace WindowsFormsApp31_32;

internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new DeliveryCostForm());
    }
}

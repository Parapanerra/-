namespace PostService.App;

internal static class Program
{
    private static ServerProcess? _server;

    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();
        _server = ServerProcess.Start();
        Application.ApplicationExit += (_, _) => _server?.Dispose();
        Application.Run(new PostingList());
    }
}

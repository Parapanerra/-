using System.Diagnostics;
using System.Reflection;

namespace PostService.App;

internal static class Program
{
    private static bool _verifyUnhandled;
    [STAThread]
    private static void Main(string[] args)
    {
        _verifyUnhandled = args.Contains("--verify-unhandled");
        AppDomain.CurrentDomain.UnhandledException += OnUnhandledException;
        Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
        ApplicationConfiguration.Initialize();
        var address = Environment.GetEnvironmentVariable("POSTSERVICE_URL") ?? "http://localhost:8046/";
        using var client = new PostServiceClient(new Uri(address));
        // Diagnostic subprocess: intentionally let an actual API error reach the global handler.
        if (_verifyUnhandled) client.GetPostingsAsync().GetAwaiter().GetResult();
        Application.Run(new PostServiceWindow(client));
    }

    private static void OnUnhandledException(object sender, UnhandledExceptionEventArgs args)
    {
        var error = args.ExceptionObject as Exception ?? new Exception(args.ExceptionObject?.ToString());
        while (error is TargetInvocationException { InnerException: not null }) error = error.InnerException;
        var entry = $"{DateTimeOffset.Now:O}\n{error}\n";
        Debug.WriteLine(entry);
        Trace.WriteLine(entry);
        var directory = _verifyUnhandled
            ? Path.Combine(AppContext.BaseDirectory, "verification")
            : Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PostService", "PR46");
        var saved = false;
        try
        {
            Directory.CreateDirectory(directory);
            File.AppendAllText(Path.Combine(directory, "errors.log"), entry);
            saved = true;
        }
        catch (Exception logError) when (logError is IOException or UnauthorizedAccessException)
        {
            Debug.WriteLine(logError);
        }
        if (_verifyUnhandled) return;
        MessageBox.Show(saved
            ? "Сталася непередбачена помилка. Програму буде закрито. Подробиці збережено в журналі PostService/PR46 у LocalAppData."
            : "Сталася непередбачена помилка. Програму буде закрито. Зверніться до розробника.",
            "Помилка PostService", MessageBoxButtons.OK, MessageBoxIcon.Error);
        // UnhandledException is a last chance to report, not a recovery mechanism.
    }
}

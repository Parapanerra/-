using System.Diagnostics;

namespace PostService.App;

internal sealed class ServerProcess : IDisposable
{
    private readonly Process _process;

    private ServerProcess(Process process) => _process = process;

    public static ServerProcess? Start()
    {
        // Вебзастосунок може вже працювати окремо. Не запускаємо другий сервер
        // на тому самому порту: це відкривало системне вікно помилки.
        try
        {
            using var client = new System.Net.Sockets.TcpClient();
            client.Connect("localhost", 5211);
            return null;
        }
        catch (System.Net.Sockets.SocketException)
        {
            // Порт вільний, запускаємо локальний PostService нижче.
        }

        var projectPath = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "..", "PostService", "PostService.csproj"));

        if (!File.Exists(projectPath)) return null;

        var startInfo = new ProcessStartInfo("dotnet")
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = Directory.GetParent(Path.GetDirectoryName(projectPath)!)!.FullName
        };
        startInfo.ArgumentList.Add("run");
        startInfo.ArgumentList.Add("--no-build");
        startInfo.ArgumentList.Add("--no-launch-profile");
        startInfo.ArgumentList.Add("--project");
        startInfo.ArgumentList.Add(projectPath);
        startInfo.ArgumentList.Add("--");
        startInfo.ArgumentList.Add("--urls");
        startInfo.ArgumentList.Add("http://localhost:5211");

        var process = Process.Start(startInfo);
        return process is null ? null : new ServerProcess(process);
    }

    public void Dispose()
    {
        if (!_process.HasExited)
        {
            _process.Kill(entireProcessTree: true);
            _process.WaitForExit(1500);
        }

        _process.Dispose();
    }
}

using System.Diagnostics;

namespace PostService.App;

internal sealed class ServerProcess : IDisposable
{
    private readonly Process _process;

    private ServerProcess(Process process) => _process = process;

    public static ServerProcess? Start()
    {
        var projectPath = Path.GetFullPath(Path.Combine(
            AppContext.BaseDirectory, "..", "..", "..", "..", "PostService", "PostService.csproj"));

        if (!File.Exists(projectPath)) return null;

        var startInfo = new ProcessStartInfo("dotnet")
        {
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = Path.GetDirectoryName(projectPath)!
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

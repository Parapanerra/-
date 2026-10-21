$ErrorActionPreference = 'Stop'
Set-Location -LiteralPath $PSScriptRoot
dotnet build PostService.App -v quiet
if ($LASTEXITCODE -ne 0) { throw 'Build failed.' }
$serverDll = Join-Path $PSScriptRoot 'PostService\bin\Debug\net10.0\PostService.dll'
$env:ASPNETCORE_ENVIRONMENT = 'Production'
$env:POSTSERVICE_URL = 'http://localhost:8046/'
$env:POSTSERVICE_TEST_FAULT_FILE = $null
$listener = Get-NetTCPConnection -LocalPort 8046 -State Listen -ErrorAction SilentlyContinue
if (!$listener) {
    Start-Process dotnet -ArgumentList ('"' + $serverDll + '" --urls http://localhost:8046') -WorkingDirectory (Join-Path $PSScriptRoot 'PostService') -WindowStyle Hidden
}
# The client is an interactive application and is shown to the user.
Start-Process -FilePath (Join-Path $PSScriptRoot 'PostService.App\bin\Debug\net10.0-windows\PostService.App.exe')

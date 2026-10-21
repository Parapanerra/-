$ErrorActionPreference = 'Stop'
Set-Location -LiteralPath $PSScriptRoot
$docker = 'C:\Program Files\Docker\Docker\resources\bin\docker.exe'
if (!(Test-Path -LiteralPath $docker)) { throw 'Docker Desktop не встановлено.' }
& $docker info *> $null
if ($LASTEXITCODE -ne 0) { throw 'Запустіть Docker Desktop і дочекайтеся запуску Engine. Якщо компоненти WSL щойно увімкнено, перезавантажте Windows.' }
& $docker compose up -d --build
if ($LASTEXITCODE -ne 0) { throw 'Не вдалося зібрати або запустити контейнери.' }
New-Item -ItemType Directory -Path verification -Force | Out-Null
& $docker images | Out-File verification/images.txt -Encoding utf8
& $docker compose ps | Out-File verification/containers.txt -Encoding utf8
& $docker compose logs --no-color | Out-File verification/logs.txt -Encoding utf8
$ready = $false
for ($attempt = 0; $attempt -lt 30; $attempt++) {
    try {
        $response = Invoke-WebRequest 'http://localhost:8080/postings' -UseBasicParsing
        if ($response.StatusCode -eq 200) { $ready = $true; break }
    } catch { Start-Sleep -Seconds 2 }
}
if (!$ready) { throw 'Контейнери створено, але API не відповів. Перевірте verification/logs.txt.' }
$response.Content | Out-File verification/postings.json -Encoding utf8
Start-Process 'http://localhost:8080'

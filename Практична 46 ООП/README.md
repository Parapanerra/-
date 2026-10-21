# Практична робота 46 з ООП

Windows Forms клієнт PostService із локальною обробкою очікуваних помилок і глобальним журналюванням непередбачених винятків.

Запуск: `Запустити.cmd` або `Запустити.ps1`. Потрібні Windows та .NET SDK 10. Скрипт збирає проєкт, запускає сервер на `http://localhost:8046` і відкриває клієнт. Для іншої адреси клієнта використовуйте змінну `POSTSERVICE_URL`.

Основні файли:

- `PostService.App/PostServiceClient.cs` — п’ять HTTP-операцій, статуси, JSON і тайм-аут.
- `PostService.App/PostingValidationException.cs` — помилки за полями.
- `PostService.App/PostServiceWindow.cs` — Retry/Cancel, оновлення після 404, повідомлення 400 і відновлення кнопок.
- `PostService.App/Program.cs` — глобальний обробник та журнал.
- `requests.rest` — запити для дослідження відповідей.
- `PostService.Checks` — 15 перевірок клієнта.
- `Відповіді на запитання.txt` — короткі відповіді.

Для тестів запустіть сервер у середовищі Testing:

```powershell
$env:ASPNETCORE_ENVIRONMENT = 'Testing'
dotnet run --project PostService --no-launch-profile --urls http://localhost:8046
```

В іншому терміналі:

```powershell
dotnet run --project PostService.Checks
```

Тільки в Testing сервер підтримує заголовок `X-PR46-Fault: 500` або `503`. За наявності змінної `POSTSERVICE_TEST_FAULT_FILE` він також читає з указаного файла код `400`, `500` або `503`. Порожній файл відновлює звичайну роботу. Для `400` тестова відповідь повертається лише на POST/PUT. Це дозволяє перевірити обробку відхилених сервером даних, які пройшли клієнтську перевірку.

Діагностичний запуск клієнта `--verify-unhandled` виконує отримання списку перед відкриттям форми. Необроблена помилка журналюється в `verification/errors.log` біля виконуваного файла; діалог у цьому спеціальному режимі не показується. Звичайний режим використовує `%LOCALAPPDATA%/PostService/PR46/errors.log` і показує повідомлення.

Під час перевірки UI потрібно окремо перевірити Retry/Cancel, збереження відкритої форми після 400, оновлення після 404 та повідомлення з подальшим завершенням після 500. Тестові записи створюйте окремо від наявних.

Успадкований пакет SQLitePCLRaw.lib.e_sqlite3 2.1.10 має попередження NU1903; воно не приглушується.

Джерела: прикріплена методичка; [глобальні винятки .NET](https://learn.microsoft.com/en-us/dotnet/fundamentals/runtime-libraries/system-appdomain-unhandledexception), [режим винятків Windows Forms](https://learn.microsoft.com/en-us/dotnet/api/system.windows.forms.application.setunhandledexceptionmode?view=windowsdesktop-10.0).

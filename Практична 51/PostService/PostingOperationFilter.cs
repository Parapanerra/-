using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PostService.Documentation;

internal sealed class PostingOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var parameter in operation.Parameters)
            if (parameter.Name == "id")
                parameter.Description = "Цілочисельний ідентифікатор наявного відправлення. Для PUT має збігатися з полем id у JSON.";
        if (operation.RequestBody is not null)
        {
            operation.RequestBody.Description = context.ApiDescription.HttpMethod == "POST"
                ? "JSON із даними нового відправлення. Поля from, to, content, deliveryType, weight, width, height і depth обов’язкові."
                : "JSON із повним набором полів відправлення. Поле id має збігатися з параметром маршруту.";
            operation.RequestBody.Required = true;
        }
        foreach (var response in operation.Responses)
            response.Value.Description = response.Key switch
            {
                "200" => "Запит виконано успішно. Відповідь містить дані відправлення або список відправлень.",
                "201" => "Відправлення створено. Тіло містить створений запис; Location указує адресу ресурсу.",
                "204" => "Відправлення видалено. Тіло відповіді відсутнє.",
                "400" => "Некоректний JSON, параметр маршруту, значення полів або невідповідність ID. Помилки валідації повертаються у форматі ValidationProblem; невідповідність ID — рядком.",
                "404" => "Відправлення з указаним ID не знайдено.",
                _ => response.Value.Description
            };
    }
}

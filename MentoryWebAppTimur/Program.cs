var builder = WebApplication.CreateBuilder();
var app = builder.Build();

//app.Map("/api/user", UserApiHandler);
//app.Map("/form", FormHandler);
app.Run(RoutingHandler);
app.Run();

async Task UserApiHandler(HttpContext context)
{
    var message = "Некорректные данные";   // содержание сообщения по умолчанию
    try
    {
        // пытаемся получить данные json
        var person = await context.Request.ReadFromJsonAsync<Person>();
        if (person != null) // если данные сконвертированы в Person
            message = $"Name: {person.Name}  Age: {person.Age}";
    }
    catch { }
    // отправляем пользователю данные
    await context.Response.WriteAsJsonAsync(new { text = message });
}

async Task FormHandler(HttpContext context)
{
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync("html/form.html");
}

async Task MainHandler(HttpContext context)
{
    context.Response.ContentType = "text/html";
    await context.Response.SendFileAsync("html/index.html");
}
async Task ImageHandler(HttpContext context)
{
    await context.Response.SendFileAsync("res/image.jpg");
}
async Task DownloadImageHandler(HttpContext context)
{
    context.Response.Headers.ContentDisposition = "attachment; filename=image.jpg";
    await context.Response.SendFileAsync("res/image.jpg");
}
async Task RoutingHandler(HttpContext context)
{
    var path = context.Request.Path;
    switch (path)
    {
        case "/main":
            await MainHandler(context); break;
        case "/api/user":
            await UserApiHandler(context); break;
        case "/form":
            await FormHandler(context); break;
        case "/image":
            await ImageHandler(context); break;
        case "/getImage":
            await DownloadImageHandler(context); break;
        case "/":
            context.Response.Redirect("/main");break;
        default:
            context.Response.StatusCode = 404;
            await context.Response.WriteAsync("Page not found!");
            break;
    }

}
public record Person(string Name, int Age);
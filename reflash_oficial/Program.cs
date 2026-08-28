using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using reflash_oficial.Controllers;
using System.Text;
using System.Text.RegularExpressions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

// 1. Статические файлы из общей папки C:\fotos
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(@"C:\fotos"),
    RequestPath = "/shared-fotos",
    ServeUnknownFileTypes = true
});

// 2. Стандартные статические файлы из wwwroot
app.UseStaticFiles();

app.UseAuthorization();

// ==========================================
// МИДЛВЕР ДЛЯ НОРМАЛИЗАЦИИ URL
// ==========================================
app.Use(async (context, next) =>
{
    var path = context.Request.Path.Value;

    if (!string.IsNullOrEmpty(path) && path.Length > 1)
    {
        try
        {
            // 1. Декодируем URL
            var decodedPath = Uri.UnescapeDataString(path);

            // 2. Транслитерация кириллицы в латиницу
            decodedPath = Transliterate(decodedPath);

            // 3. Заменяем пробелы на _
            decodedPath = decodedPath.Replace(' ', '_');

            // 4. Заменяем множественные _ на один
            decodedPath = Regex.Replace(decodedPath, "_+", "_");

            // 5. Удаляем _ в начале и конце
            decodedPath = decodedPath.Trim('_');

            // Если путь изменился - обновляем
            if (decodedPath != path)
            {
                context.Request.Path = decodedPath;
            }
        }
        catch
        {
            // Если обработка не удалась - пропускаем
        }
    }

    await next();
});

// ==========================================
// ФУНКЦИЯ ТРАНСЛИТЕРАЦИИ
// ==========================================
static string Transliterate(string text)
{
    if (string.IsNullOrEmpty(text)) return text;

    var map = new Dictionary<char, string>
    {
        // Кириллица -> Латиница
        {'а', "a"}, {'б', "b"}, {'в', "v"}, {'г', "g"}, {'д', "d"}, {'е', "e"},
        {'ё', "e"}, {'ж', "zh"}, {'з', "z"}, {'и', "i"}, {'й', "y"}, {'к', "k"},
        {'л', "l"}, {'м', "m"}, {'н', "n"}, {'о', "o"}, {'п', "p"}, {'р', "r"},
        {'с', "s"}, {'т', "t"}, {'у', "u"}, {'ф', "f"}, {'х', "h"}, {'ц', "ts"},
        {'ч', "ch"}, {'ш', "sh"}, {'щ', "sch"}, {'ъ', ""}, {'ы', "y"}, {'ь', ""},
        {'э', "e"}, {'ю', "yu"}, {'я', "ya"},
        {'А', "a"}, {'Б', "b"}, {'В', "v"}, {'Г', "g"}, {'Д', "d"}, {'Е', "e"},
        {'Ё', "e"}, {'Ж', "zh"}, {'З', "z"}, {'И', "i"}, {'Й', "y"}, {'К', "k"},
        {'Л', "l"}, {'М', "m"}, {'Н', "n"}, {'О', "o"}, {'П', "p"}, {'Р', "r"},
        {'С', "s"}, {'Т', "t"}, {'У', "u"}, {'Ф', "f"}, {'Х', "h"}, {'Ц', "ts"},
        {'Ч', "ch"}, {'Ш', "sh"}, {'Щ', "sch"}, {'Ъ', ""}, {'Ы', "y"}, {'Ь', ""},
        {'Э', "e"}, {'Ю', "yu"}, {'Я', "ya"}
    };

    var result = new StringBuilder();
    foreach (char c in text)
    {
        if (map.TryGetValue(c, out string? replacement))
            result.Append(replacement);
        else
            result.Append(c);
    }

    return result.ToString();
}

// ==========================================
// ЭНДПОИНТ ДЛЯ ПРИЁМА СИГНАЛОВ ОТ АДМИНКИ
// ==========================================
app.Map("/api/db-notify", async (HttpContext context) =>
{
    try
    {
        if (context.Request.Method == "POST" || context.Request.Method == "GET")
        {
            Console.WriteLine($"🔔 [{DateTime.Now}] Сигнал получен через {context.Request.Method}");

            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("{\"success\":true,\"message\":\"Cache cleared\"}");
        }
        else
        {
            context.Response.StatusCode = 405;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("{\"error\":\"Method not allowed\"}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ [{DateTime.Now}] Ошибка: {ex.Message}");
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync($"{{\"error\":\"{ex.Message}\"}}");
    }
});

// ==========================================
// МАРШРУТ ДЛЯ СТАРЫХ URL 
// ==========================================
app.MapControllerRoute(
    name: "car_by_old",
    pattern: "cars/{**oldUrl}",
    defaults: new { controller = "Home", action = "CarByOldUrl" });


app.MapControllerRoute(
    name: "partners",
    pattern: "Partners",
    defaults: new { controller = "Home", action = "Partners" });

app.MapControllerRoute(
    name: "brand_cars",
    pattern: "/{brand}",
    defaults: new { controller = "Home", action = "CarsByBrand" },
    constraints: new { brand = @".+" });

app.MapControllerRoute(
    name: "car_details",
    pattern: "/{brand}/{model}/{generation}/{engine}",
    defaults: new { controller = "Home", action = "Car" });

app.MapControllerRoute(
    name: "short",
    pattern: "{action=Index}/{id?}",
    defaults: new { controller = "Home" });

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
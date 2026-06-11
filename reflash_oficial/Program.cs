using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using reflash_oficial.Controllers;

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
// ЭНДПОИНТ ДЛЯ ПРИЁМА СИГНАЛОВ ОТ АДМИНКИ
// ==========================================
app.Map("/api/db-notify", async (HttpContext context) =>
{
    try
    {
        // Принимаем POST и GET запросы
        if (context.Request.Method == "POST" || context.Request.Method == "GET")
        {
            Console.WriteLine($"🔔 [{DateTime.Now}] Сигнал получен через {context.Request.Method}");

            // Сбрасываем кэш данных
            HomeController.RefreshData();

            Console.WriteLine($"✅ [{DateTime.Now}] Кэш успешно очищен");

            // Возвращаем успешный ответ
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("{\"success\":true,\"message\":\"Cache cleared\"}");
        }
        else
        {
            // Неподдерживаемый метод
            context.Response.StatusCode = 405;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync("{\"error\":\"Method not allowed\"}");
        }
    }
    catch (Exception ex)
    {
        // Ошибка при очистке кэша
        Console.WriteLine($"❌ [{DateTime.Now}] Ошибка: {ex.Message}");
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync($"{{\"error\":\"{ex.Message}\"}}");
    }
});

app.MapControllerRoute(
    name: "car_details",
    pattern: "/{brand}/{model}/{generation}/{engine}",
    defaults: new { controller = "Home", action = "Car" });

app.MapControllerRoute(
    name: "short",
    pattern: "{action=Index}/{id?}",
    defaults: new { controller = "Home" });

// Стандартные маршруты для контроллеров
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();
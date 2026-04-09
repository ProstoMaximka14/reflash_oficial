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

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(@"C:\fotos"),
    RequestPath = "/shared-fotos",
    ServeUnknownFileTypes = true // Разрешаем все типы файлов
});

// 2. Затем стандартные статические файлы из wwwroot
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapPost("/api/db-notify", () =>
{
    Console.WriteLine($"🔔 [{DateTime.Now}] ПОЛУЧЕН СИГНАЛ: База данных изменена!");
    HomeController.RefreshData();
    return Results.Ok();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

using BackEnd;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddSingleton<IDatos, Datos>();

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession();

var app = builder.Build();

app.UseStaticFiles();

app.UseSession();

app.MapRazorPages();

app.Run();

var builder = WebApplication.CreateBuilder(args);

// DI blazor page service
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddOpenApi();

var app = builder.Build();

// Cấu hình routing
app.UseRouting();

// Cấu hình Blazor Hub
app.MapBlazorHub();

// Cấu hình trang chủ mặc định
app.MapFallbackToPage("/_Host");

// cau hinh trang chu ma dinh
app.MapFallbackToPage("/_Host");
app.UseStaticFiles();

app.Run();

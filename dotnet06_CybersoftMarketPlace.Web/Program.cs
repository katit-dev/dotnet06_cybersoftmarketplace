var builder = WebApplication.CreateBuilder(args);

// DI blazor page service
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

builder.Services.AddOpenApi();



// DI http client service
builder.Services.AddHttpClient("CybersoftMarketPlace.Web.ServerAPI", client =>
{
    client.BaseAddress = new Uri("https://localhost:5269");
});

// DI Service State management
builder.Services.AddScoped<ProductStateService>();

var app = builder.Build();




// Cấu hình routing
app.UseRouting();

// Cấu hình Blazor Hub
app.MapBlazorHub();

// Cấu hình trang chủ mặc định
app.MapFallbackToPage("/_Host");

app.UseStaticFiles();

app.Run();

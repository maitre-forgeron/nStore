using NStore.Logging;
using NStore.Web.Extensions;
using NStore.Web.HttpHandlers;
using NStore.Web.Services;
using NStore.Web.Services.Base;
using NStore.Web.Services.Interfaces;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog(SeriLogger.Configure);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddTransient<AuthenticationDelegatingHandler>();

builder.Services.AddHttpClient<HttpClientService>(options =>
{
    var url = builder.Configuration.GetValue<string>("Gateway");

    options.BaseAddress = new Uri(url);
})
    .AddUserAccessTokenHandler()
    .AddHttpMessageHandler<AuthenticationDelegatingHandler>()
    .AddStandardResilienceHandler();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<ICategoryService, CategoryService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();

builder.Services.ConfigureAuthentication(builder.Configuration);

builder.Services.AddHttpsRedirection(options =>
{
    options.HttpsPort = 443;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseForwardedHeaders();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
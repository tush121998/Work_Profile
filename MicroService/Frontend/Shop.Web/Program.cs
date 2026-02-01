using Microsoft.AspNetCore.Authentication.Cookies;
using Shop.Web.Service;
using Shop.Web.Service.IService;
using Shop.Web.Utility;

var builder = WebApplication.CreateBuilder(args);

// MVC
builder.Services.AddControllersWithViews();
builder.Services.AddHttpContextAccessor();
// Set API Base URL
SD.CouponApiBase = builder.Configuration.GetValue<string>("ServiceUrls:CouponAPI");
SD.AuthApiBase = builder.Configuration.GetValue<string>("ServiceUrls:AuthAPI");
SD.ProductApiBase = builder.Configuration.GetValue<string>("ServiceUrls:ProductAPI");
SD.CartApiBase = builder.Configuration.GetValue<string>("ServiceUrls:CartAPI");
// Register BaseService HttpClient
builder.Services.AddHttpClient<IAuthService, AuthService>();
builder.Services.AddHttpClient<ICouponService, CouponService>();
builder.Services.AddHttpClient<IProductService, ProductService>();
builder.Services.AddHttpClient<ICartService, CartService>();

// DI services
builder.Services.AddScoped<ITokenProvider, TokenProvider>();
builder.Services.AddScoped<IBaseService, BaseService>();
builder.Services.AddScoped<ICouponService, CouponService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICartService, CartService>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromMinutes(3);
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
    });
var app = builder.Build();

// Pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using UdemyCarBook.Persistence.Context;
using UdemyCarBook.Persistence.Seed; // 🔥 SEED EKLENDİ

var builder = WebApplication.CreateBuilder(args);

// Controllers + Views
builder.Services.AddControllersWithViews();

// HttpClient
builder.Services.AddHttpClient();


// 🗄️ DATABASE (EF CORE)
builder.Services.AddDbContext<CarBookContext>(options =>
{
    options.UseSqlServer("Server=DESKTOP-OCK4D7G;initial Catalog=UdemyCarBookDb;integrated Security=true;TrustServerCertificate=true;");
});


// 🔐 AUTH (Cookie Authentication)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(opt =>
    {
        opt.LoginPath = "/Login/Index";
        opt.LogoutPath = "/Login/LogOut";
        opt.AccessDeniedPath = "/Pages/AccessDenied";

        opt.Cookie.Name = "CarBookAuth";
        opt.Cookie.HttpOnly = true;
        opt.Cookie.SameSite = SameSiteMode.Lax;
        opt.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
    });

var app = builder.Build();


// ===============================
// 🔥 SEED DATA ÇALIŞTIRMA (EKLENDİ)
// ===============================
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<CarBookContext>();

    context.Database.Migrate(); // DB yoksa oluşturur

    SeedData.Initialize(context); // 🔥 admin + role ekler
}


// Hata sayfası
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


// 🟢 AREA ROUTE
app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
);

// 🟢 DEFAULT ROUTE
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}"
);

app.Run();
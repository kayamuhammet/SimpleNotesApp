using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SimpleNotesApp.Data;
using SimpleNotesApp.Models;
using SimpleNotesApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add localisation services
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");


// database connections -> sqlite
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite(builder.Configuration.GetConnectionString("database")));

// add the identity service and configure user authentication settings
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options => {
    
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 6;
    
    // Email confirmation settings
    options.SignIn.RequireConfirmedEmail = true;
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

// Add email service
builder.Services.AddTransient<IEmailService, EmailService>();

// cookie-based authentication settings
builder.Services.ConfigureApplicationCookie(options => {
    options.LoginPath = "/Account/Login";
    options.LogoutPath = "/Account/Logout";
    options.AccessDeniedPath = "/Account/AccessDenied";
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
    options.SlidingExpiration = true;
});

// Configure the authentication process
builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "smart";
    options.DefaultChallengeScheme = "smart";
})
.AddCookie(CookieAuthenticationDefaults.AuthenticationScheme)
.AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidAudience = builder.Configuration["JWT:ValidAudience"],
        ValidIssuer = builder.Configuration["JWT:ValidIssuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Secret"]))
    };
})
.AddPolicyScheme("smart", "Smart Scheme", options =>
{
    // api calls or requests containing an Authorization header will use JWT authentication
    options.ForwardDefaultSelector = context =>
    {
        
        if (context.Request.Path.StartsWithSegments("/api") || 
            context.Request.Headers.ContainsKey("Authorization"))
        {
            return JwtBearerDefaults.AuthenticationScheme; // use JWT in API requests
        }
        return CookieAuthenticationDefaults.AuthenticationScheme; // use cookie authentication on others
    };
});



// Add services to the container.
builder.Services.AddControllersWithViews()
    .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
    .AddDataAnnotationsLocalization();

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "tr-tr", "en-us" };
    options.SetDefaultCulture("tr-tr")
           .AddSupportedCultures(supportedCultures)
           .AddSupportedUICultures(supportedCultures);
    options.RequestCultureProviders.Insert(0, new CookieRequestCultureProvider()
    {
        CookieName = "CULTURE"
    });
});

builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseUrls = true;
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}


app.UseStatusCodePagesWithReExecute("/Error/{0}");

// Localization middleware
var locOptions = app.Services.GetRequiredService<IOptions<RequestLocalizationOptions>>();
app.UseRequestLocalization(locOptions.Value);

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{lang=tr-tr}/{controller=Home}/{action=Index}/{id?}",
    constraints: new { lang = "tr-tr|en-us" });

app.MapControllerRoute(
    name: "catch-all-with-lang",
    pattern: "{lang}/{*path}",
    constraints: new { lang = "tr-tr|en-us" },
    defaults: new { controller = "Error", action = "HandleError", statusCode = 404 });

app.MapControllerRoute(
    name: "catch-all",
    pattern: "{*path}",
    defaults: new { controller = "Error", action = "HandleError", statusCode = 404 });


app.Run();

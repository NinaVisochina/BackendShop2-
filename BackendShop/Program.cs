
using BackendShop.BackShop;
using BackendShop.BackShop.Extensions;
using BackendShop.Core;
using BackendShop.Core.Interfaces;
using BackendShop.Core.Services;
using BackendShop.Data;
using BackendShop.Data.Data;
using BackendShop.Data.DataSeeder;
using BackendShop.Services;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("DefaultConnection")!;

// Add services to the container.

builder.Services.AddControllers();
//builder.Services.AddDbContext<ShopDbContext>(options =>
    //options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext(connectionString);
builder.Services.AddIdentity();
builder.Services.AddRepository();

// fluent validators

builder.Services.AddHttpContextAccessor();
builder.Services.AddAutoMapper();
builder.Services.AddFluentValidators();

// custom services
builder.Services.AddCustomServices();

// exception handlers
builder.Services.AddExceptionHandler();

builder.Services.AddJWT(builder.Configuration);
builder.Services.AddSwaggerJWT();
builder.Services.AddHangfire(connectionString);

builder.Services.AddCorsPolicies();

var app = builder.Build();

// -------------- Seed Initial Data
//using (var scope = app.Services.CreateScope())
//{
//    scope.ServiceProvider.SeedRoles().Wait();
//    scope.ServiceProvider.SeedAdmin().Wait();
//}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
if (app.Environment.IsProduction())
{
    app.UseExceptionHandler();
}
string imagesDirPath = Path.Combine(Directory.GetCurrentDirectory(), builder.Configuration["ImagesDir"]);
if (!Directory.Exists(imagesDirPath))
{
    Directory.CreateDirectory(imagesDirPath);
}
app.UseHttpsRedirection();

app.UseCors("front-end-cors-policy");

app.UseAuthentication();
app.UseAuthorization();
// Додає обробку статичних файлів
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(imagesDirPath),
    RequestPath = "/images"
});
app.UseHangfireDashboard("/dash");
JobConfigurator.AddJobs();
app.SeedDataAsync();
app.MapControllers();
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await services.SeedRoles();
    await services.SeedAdmin();
}
app.Run();
//using BackendShop.Core.Interfaces;
//using BackendShop.Data.Data;
//using BackendShop.Data.DataSeeder;
//using BackendShop.Services;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.FileProviders;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.IdentityModel.Tokens;
//using System.Text;
//using BackendShop.Core.Services;

//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();
//builder.Services.AddDbContext<ShopDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
////builder.Services.ConfigureApplicationCookie(options =>
////{
////    options.LoginPath = "/Identity/Account/Login";
////    options.LogoutPath = "/Identity/Account/Logout";
////    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
////});

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//})
//.AddJwtBearer(options =>
//{
//    options.TokenValidationParameters = new TokenValidationParameters
//    {
//        ValidateIssuer = true,
//        ValidateAudience = true,
//        ValidateLifetime = true,
//        ValidateIssuerSigningKey = true,
//        ValidIssuer = "YourIssuer", // Вкажіть ваш Issuer
//        ValidAudience = "YourAudience", // Вкажіть вашу Audience
//        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("YourSecretKey")) // Секретний ключ
//    };
//});
//builder.Services.AddIdentity<IdentityUser, IdentityRole>()
//    .AddEntityFrameworkStores<ShopDbContext>()
//    .AddDefaultTokenProviders();

//builder.Services.AddControllersWithViews();

//builder.Services.AddAutoMapper(typeof(Program).Assembly);
//builder.Services.AddScoped<IImageHulk, ImageHulk>();
//builder.Services.AddScoped<ICategoryService, CategoryService>();
//builder.Services.AddScoped<ISubCategoryService, SubCategoryService>();
//builder.Services.AddScoped<IProductService, ProductService>();
//builder.Services.AddScoped<ICartService, CartService>();

//var app = builder.Build();

//app.UseCors(opt =>
//    opt.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//string imagesDirPath = Path.Combine(Directory.GetCurrentDirectory(), builder.Configuration["ImagesDir"]);

//if (!Directory.Exists(imagesDirPath))
//{
//    Directory.CreateDirectory(imagesDirPath);
//}

//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(imagesDirPath),
//    RequestPath = "/images"
//});


//app.SeedDataAsync();
//app.UseAuthentication();
//app.UseAuthorization();

//app.MapControllers();

//app.Run();

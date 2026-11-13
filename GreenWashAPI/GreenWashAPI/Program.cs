using System.Text;
using GreenWashAPI.Interface;
using GreenWashAPI.Models;
using GreenWashAPI.Repository;
using GreenWashAPI.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MyApp.Data;

var builder = WebApplication.CreateBuilder(args); 

// Add services to the container
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
            builder => builder.AllowAnyOrigin()
                             .AllowAnyMethod()
                             .AllowAnyHeader());// Allow any methods (GET, POST, etc.)
});

// Add DB Context for ApplicationDbContext (assuming it's for Identity or other purposes)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add DB Context for GreenWashDbContext (main application DB context)
builder.Services.AddDbContext<GreenWashDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add your repositories for dependency injection
builder.Services.AddScoped<IUsers, UserRepo>();
builder.Services.AddScoped<ICars, CarRepo>();
builder.Services.AddScoped<IOrders, OrderRepo>();
builder.Services.AddScoped<IPackages, PackageRepo>();
//builder.Services.AddScoped<IPayments, PaymentsRepo>();
builder.Services.AddScoped<IAddOns, AddOnRepo>();
builder.Services.AddScoped<IReviews, ReviewRepo>();
builder.Services.AddScoped<INotifications, NotificationRepo>();
builder.Services.AddScoped<IOrderAddOns, OrderAddOnRepo>();
builder.Services.AddScoped<IPromoCodes, PromoCodeRepo>();
builder.Services.AddScoped<ILeaderboards, LeaderboardRepo>();

// Add controllers and other services
builder.Services.AddControllers();

builder.Services.AddHttpClient();
// Register the JwtService for token creation and validation
builder.Services.AddScoped<JwtService>();

// Configure JWT Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var secretKey = builder.Configuration["Jwt:Key"];
    var key = Encoding.ASCII.GetBytes(secretKey);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

// Add Authorization services
builder.Services.AddAuthorization(options =>
{
    // Define roles and policies as necessary, for example:
    options.AddPolicy("AdminPolicy", policy => policy.RequireRole("Admin"));
    options.AddPolicy("WasherPolicy", policy => policy.RequireRole("Washer"));
    options.AddPolicy("CustomerPolicy", policy => policy.RequireRole("Customer"));
    //  options.AddPolicy("AdminCustomerPolicy", policy => policy.RequireRole("Customer").RequireRole("Admin"));
    options.AddPolicy("AdminCustomerPolicy", policy =>
         policy.RequireAssertion(context =>
             context.User.IsInRole("Admin") || context.User.IsInRole("Customer")));
    options.AddPolicy("AdminWasherPolicy", policy =>
         policy.RequireAssertion(context =>
             context.User.IsInRole("Admin") || context.User.IsInRole("Washer")));
    options.AddPolicy("WasherCustomerPolicy", policy =>
         policy.RequireAssertion(context =>
             context.User.IsInRole("Washer") || context.User.IsInRole("Customer")));
    options.AddPolicy("AdminWasherCustomerPolicy", policy =>
         policy.RequireAssertion(context =>
             context.User.IsInRole("Admin") || context.User.IsInRole("Washer") || context.User.IsInRole("Customer")));
});

// Add Swagger for API documentation
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseHttpsRedirection();

// Enable Authentication and Authorization middlewares
app.UseAuthentication();
app.UseAuthorization();

// Map controllers (this assumes you have API controllers)
app.MapControllers();

app.Run();

using Microsoft.EntityFrameworkCore;

using PaymentService.Interfaces;
//using PaymentService.Repository;
//using PaymentService.Interface;
//using PaymentService.Models;

var builder = WebApplication.CreateBuilder(args);
// Use the same connection string as the CarWashAPI project.

//builder.Services.AddDbContext<PaymentDbContext>(options =>
//    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
// Register PaymentService implementation.
builder.Services.AddScoped<PaymentRepo>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
var app = builder.Build();
// Enable Swagger in development.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();







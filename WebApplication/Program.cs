using Microsoft.EntityFrameworkCore;
using WebApp.Data;
using WebApp.Extensions;
using WebApp.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins("http://localhost:4200") // Дозволити Angular клієнт
              .AllowAnyMethod()                     // Дозволити всі методи
              .AllowAnyHeader()                     // Дозволити всі заголовки
              .AllowCredentials();                  // Дозволити обмін обліковими даними
    });
});
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSignalR();
builder.Services.AddApplicationServices();
builder.Services.AddAutoMapper(typeof(Program));

var app = builder.Build();

app.UseCors("AllowAngularApp");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapHub<MarketDataHub>("/marketDataHub");
app.MapControllers();

var marketDataService = app.Services.GetRequiredService<MarketDataStreamingService>();
await marketDataService.StartStreaming();

app.Run();

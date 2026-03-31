using ItineraryOperations.Models;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);
Console.OutputEncoding = System.Text.Encoding.UTF8;

builder.Services.AddDbContext<PostgresContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Host=localhost;Port=5433;Database=itineraryOperationDb;Username=postgres;Password=546915")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "ItineraryOperations API",
        Version = "v1",
        Description = "API которое меня за*****"
    });

    c.EnableAnnotations();  // Для SwaggerResponse атрибутов (тайтл)
    c.ExampleFilters(); // Для SwaggerResponseExample атрибутов (пример)
});
builder.Services.AddSwaggerExamplesFromAssemblyOf<APIError>();
builder.Logging.AddDebug(); // Для вывода в Debug-окно

// Добавление CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});
var app = builder.Build();

// Использование CORS
app.UseCors("AllowSpecificOrigin");


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();



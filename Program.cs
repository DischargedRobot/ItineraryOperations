using ItineraryOperations.Models;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Filters;
using DotNetEnv;

// Загружаем переменные окружения из .env файла
DotNetEnv.Env.Load();

var dbHost = Environment.GetEnvironmentVariable("DB_HOST") ?? "localhost";
var dbPort = Environment.GetEnvironmentVariable("DB_PORT") ?? "5433";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "itineraryOperationDb";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "";
var dbSchema = Environment.GetEnvironmentVariable("DB_SCHEMA") ?? "public";

var connectionString = $"Host={dbHost};Port={dbPort};Database={dbName};Username={dbUser};Password={dbPassword};SearchPath={dbSchema}";

var builder = WebApplication.CreateBuilder(args);
Console.OutputEncoding = System.Text.Encoding.UTF8;

builder.Services.AddDbContext<PostgresContext>(options =>
    options.UseNpgsql(connectionString));

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
        Description = "API ������� ���� ��*****"
    });

    c.EnableAnnotations();  // ��� SwaggerResponse ��������� (�����)
    c.ExampleFilters(); // ��� SwaggerResponseExample ��������� (������)
});
builder.Services.AddSwaggerExamplesFromAssemblyOf<APIError>();
builder.Logging.AddDebug(); // ��� ������ � Debug-����

// ���������� CORS
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

// ������������� CORS
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



using System.Text;
using KPO_hw.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;

// Создание объекта WebApplication с использованием CreateBuilder для настройки приложения ASP.NET Core
var builder = WebApplication.CreateBuilder(args);

// Добавление службы контроллеров в контейнер служб
builder.Services.AddControllers();
// Настройка Swagger для генерации документации API
builder.Services.AddSwaggerGen(c =>
{
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Добавление службы аутентификации с использованием схемы аутентификации JwtBearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

// Добавление службы для обнаружения и описания конечных точек API
builder.Services.AddEndpointsApiExplorer();
// Добавление службы Swagger для генерации документации API
builder.Services.AddSwaggerGen();
// Добавление службы контекста базы данных с использованием провайдера PostgreSQL
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
// Создание объекта приложения
var app = builder.Build();

// Использование Swagger и Swagger UI для просмотра и тестирования API.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Перенаправление запросов HTTP на HTTPS
app.UseHttpsRedirection();
// Использование службы аутентификации
app.UseAuthentication();
// Использование службы авторизации
app.UseAuthorization();

// Настройка маршрутизации для контроллеров
app.MapControllers();
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
// Запуск приложения
app.Run();
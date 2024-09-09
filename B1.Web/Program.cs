using B1._Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Добавляем IConfiguration для доступа к настройкам
var configuration = builder.Configuration;

// Проверка строки подключения
var connectionString = configuration.GetConnectionString("DbConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("Строка подключения не найдена.");
}

// Add services to the container.
builder.Services.AddControllers();

// Настройка подключения к базе данных из конфигурации
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// Регистрация сервиса ExcelWork
builder.Services.AddScoped<ExcelWork>();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseStaticFiles(); // Добавьте эту строку для поддержки статических файлов

app.UseAuthorization();

app.MapControllers();

app.Run();

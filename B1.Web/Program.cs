using B1._Persistence;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// ��������� IConfiguration ��� ������� � ����������
var configuration = builder.Configuration;

// �������� ������ �����������
var connectionString = configuration.GetConnectionString("DbConnection");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("������ ����������� �� �������.");
}

// Add services to the container.
builder.Services.AddControllers();

// ��������� ����������� � ���� ������ �� ������������
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));

// ����������� ������� ExcelWork
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
app.UseStaticFiles(); // �������� ��� ������ ��� ��������� ����������� ������

app.UseAuthorization();

app.MapControllers();

app.Run();

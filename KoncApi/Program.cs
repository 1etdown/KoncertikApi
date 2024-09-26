using KoncApi;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Получаем строку подключения из файла конфигурации
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Добавляем контекст базы данных с использованием PostgreSQL
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Добавляем зависимости для сервисов
builder.Services.AddScoped<IVenueService, VenueService>();

// Добавляем контроллеры и Swagger для документации API
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Конфигурация для среды разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using PracticeAPI4.DB;

var builder = WebApplication.CreateBuilder(args);

// Добавление сервисов в контейнер
builder.Services.AddDbContext<APIContext>(options
    => options.UseSqlServer(builder.Configuration.GetConnectionString("APIContext")
    ?? throw new InvalidOperationException("Connection string 'APIContext' not found")));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(s =>
{
    s.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "THIS IS BULLSHEET D:<", Version = "v1" });
});

var app = builder.Build();

// Настройка магистрали HTTP-запросов (DeepL Translate кинул меня через прогиб, довольствуйтесь Гугл-Переводчиком)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    IdentityModelEventSource.ShowPII = true;
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

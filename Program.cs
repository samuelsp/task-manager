using Microsoft.OpenApi;
using System.Reflection;
using TaskManager.Application.Service;
using TaskManager.Domain.Interfaces;
using TaskManager.Infra.Database;
using TaskManager.Infra.Database.Interfaces;
using TaskManager.Infra.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "TaskManager API",
        Version = "v1",
        Description = "API para gestão de tarefas"
    });

    // include XML comments if generated
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Repository + Service
builder.Services.AddSingleton<ISqliteConnectionFactory, SqliteConnectionFactory>();
builder.Services.AddSingleton<ITarefaRepository, TarefaRepository>();
builder.Services.AddSingleton<TarefaService>();

var app = builder.Build();

var connectionFactory = app.Services.GetRequiredService<ISqliteConnectionFactory>();
connectionFactory.EnsureDatabase();

// Enable Swagger UI (em desenvolvimento)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager API v1");
        c.RoutePrefix = "swagger"; // acesso em /swagger
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();



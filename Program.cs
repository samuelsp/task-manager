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

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});

// Database factory (singleton) and repository/service (scoped)
builder.Services.AddSingleton<ISqliteConnectionFactory, SqliteConnectionFactory>();
builder.Services.AddScoped<ITarefaRepository, TarefaRepository>();
builder.Services.AddScoped<ITarefaService, TarefaService>();

var app = builder.Build();

// Garantir esquema antes de aceitar requisições — resolver via scope
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetService<ILogger<Program>>();
    try
    {
        var connectionFactory = scope.ServiceProvider.GetRequiredService<ISqliteConnectionFactory>();
        connectionFactory.EnsureDatabase();
    }
    catch (Exception ex)
    {
        logger?.LogCritical(ex, "Falha ao garantir o esquema do banco de dados.");
        throw;
    }
}

// Enable Swagger UI (em desenvolvimento)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "TaskManager API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();



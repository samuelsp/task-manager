using Microsoft.OpenApi;
using System.Reflection;
using TaskManager.Application.Service;
using TaskManager.Domain.Interfaces;
using TaskManager.Infra.Repository;
using Microsoft.Data.Sqlite;

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
builder.Services.AddSingleton<ITarefaRepository, TarefaRepository>();
builder.Services.AddSingleton<TarefaService>();

var app = builder.Build();

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

var connectionString = app.Configuration.GetConnectionString("Sqlite") ?? "Data Source=tasks.db";
EnsureDatabase(connectionString);

app.Run();

static void EnsureDatabase(string connectionString)
{
    using var conn = new SqliteConnection(connectionString);
    conn.Open();
    using var cmd = conn.CreateCommand();
    cmd.CommandText = @"
    CREATE TABLE IF NOT EXISTS Tarefas (
        Id INTEGER PRIMARY KEY AUTOINCREMENT,
        Titulo TEXT NOT NULL,
        Descricao TEXT,
        Status INTEGER NOT NULL,
        DataCriacao TEXT,
        DataAlteracao TEXT
    );";
    cmd.ExecuteNonQuery();
}

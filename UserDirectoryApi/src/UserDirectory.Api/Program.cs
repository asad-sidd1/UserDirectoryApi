#nullable enable
using Serilog;
using UserDirectory.Api.Middleware;
using UserDirectory.Application;
using UserDirectory.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Serilog configuration
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();

// Add services
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Ensure data folder exists when using file-based SQLite connection
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=./data/userdirectory.db";
var dataSource = connectionString.Replace("Data Source=", "", StringComparison.OrdinalIgnoreCase).Trim();
var directory = Path.GetDirectoryName(Path.GetFullPath(dataSource));
if (!string.IsNullOrWhiteSpace(directory))
{
    Directory.CreateDirectory(directory);
}

// Ensure database migrations are applied at startup
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var db = scope.ServiceProvider.GetRequiredService<UserDirectory.Infrastructure.Persistence.UserDbContext>();
        db.Database.Migrate();
        logger.LogInformation("Database migrations applied.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Error while migrating/initializing the database.");
        // Optional: rethrow or swallow depending on desired startup behavior
        throw;
    }
}

// Pipeline
app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();

app.UseCors("ReactPolicy");
app.UseAuthorization();

app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();
app.MapHealthChecks("/health");

// Fallback: return index.html for unmatched routes (SPA)
app.MapFallbackToFile("index.html");

app.Run();

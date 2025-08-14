using System.Text;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using taskflow_api_backend.Auth;
using taskflow_api_backend.Data;
using taskflow_api_backend.Options;
using taskflow_api_backend.Repositories;
using taskflow_api_backend.Services;

var builder = WebApplication.CreateBuilder(args);

// Load .env variables early
try
{
    // Attempts to locate and load a .env file from current or parent directories
    Env.TraversePath().Load();
}
catch
{
    // Ignore if .env is missing; environment variables might be provided via other means
}

// Add Controllers
builder.Services.AddControllers();

// Add OpenAPI/Swagger with metadata
builder.Services.AddOpenApiDocument(settings =>
{
    settings.Title = "TaskFlow API";
    settings.Version = "v1";
    settings.Description = "TaskFlow API provides endpoints for user authentication and task management with JWT-based security.";
    settings.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = OpenApiSecurityApiKeyLocation.Header,
        Description = "Type into the text-box: Bearer {your JWT token}."
    });
    settings.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.SetIsOriginAllowed(_ => true)
              .AllowCredentials()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Bind Jwt settings from environment variables
var jwtSettings = new JwtSettings
{
    Secret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? string.Empty,
    Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER"),
    Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE"),
    ExpirationMinutes = int.TryParse(Environment.GetEnvironmentVariable("JWT_EXPIRATION_MINUTES"), out var mins) ? mins : 60
};

if (string.IsNullOrWhiteSpace(jwtSettings.Secret))
{
    throw new InvalidOperationException("Missing required JWT_SECRET in environment. Please set it in your .env file. See .env.example for guidance.");
}
builder.Services.AddSingleton(jwtSettings);

// Configure EF Core (SQL Server)
var sqlConnectionString = Environment.GetEnvironmentVariable("SQLSERVER_CONNECTION_STRING");
if (string.IsNullOrWhiteSpace(sqlConnectionString))
{
    throw new InvalidOperationException("Missing required SQLSERVER_CONNECTION_STRING in environment. Please set it in your .env file. See .env.example for guidance.");
}

builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseSqlServer(sqlConnectionString);
});

// Configure Authentication + JWT Bearer
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
var validateIssuer = !string.IsNullOrWhiteSpace(jwtSettings.Issuer);
var validateAudience = !string.IsNullOrWhiteSpace(jwtSettings.Audience);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ValidateIssuer = validateIssuer,
        ValidIssuer = jwtSettings.Issuer,
        ValidateAudience = validateAudience,
        ValidAudience = jwtSettings.Audience,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(30)
    };
});

// Register DI for Repositories and Services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

// Use CORS
app.UseCors("AllowAll");

// Use OpenAPI/Swagger
app.UseOpenApi();
app.UseSwaggerUi(config =>
{
    config.Path = "/docs";
    config.DocumentTitle = "TaskFlow API Docs";
});

// Use Authentication and Authorization
app.UseAuthentication();
app.UseAuthorization();

// Map Controllers
app.MapControllers();

// Health check endpoint
app.MapGet("/", () => new { message = "Healthy" })
   .WithName("HealthCheck");

// Initialize database (create if not exists)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    // In dev environments, ensure DB is created. For production, use migrations.
    try
    {
        db.Database.EnsureCreated();
    }
    catch (Exception)
    {
        // ignore creation errors - the DB might already exist or require migrations
    }
}

app.Run();

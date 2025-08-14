using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;
using TaskFlow.Api.Data;
using TaskFlow.Api.Repositories;
using TaskFlow.Api.Repositories.Interfaces;
using TaskFlow.Api.Services;
using TaskFlow.Api.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();

// OpenAPI/Swagger via NSwag with JWT support
builder.Services.AddOpenApiDocument(config =>
{
    config.Title = "TaskFlow API";
    config.Description = "TaskFlow API provides endpoints for managing tasks and users with JWT-based authentication.";
    config.Version = "v1";
    config.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
    {
        Type = OpenApiSecuritySchemeType.ApiKey,
        Name = "Authorization",
        In = OpenApiSecurityApiKeyLocation.Header,
        Description = "Type into the textbox: Bearer {your JWT token}."
    });
    config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
});

// EF Core - SQL Server
builder.Services.AddDbContext<TaskFlowDbContext>(options =>
{
    var connStr = builder.Configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connStr);
});

// CORS
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

// Authentication - JWT
var jwtKey = builder.Configuration["Jwt:Key"] ?? "CHANGE_ME_DEV_SECRET_KEY_ONLY";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "CHANGE_ME_ISSUER";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "CHANGE_ME_AUDIENCE";

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false; // Set true in production with HTTPS
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
            ClockSkew = TimeSpan.FromMinutes(1)
        };
    });

builder.Services.AddAuthorization();

// Dependency Injection - Repositories and Services
builder.Services.AddScoped<IUserRepository, EfUserRepository>();
builder.Services.AddScoped<ITaskRepository, EfTaskRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ITaskService, TaskService>();

var app = builder.Build();

// Middleware pipeline
app.UseCors("AllowAll");

app.UseOpenApi();
app.UseSwaggerUi(config =>
{
    config.Path = "/docs";
});

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health check endpoint
app.MapGet("/", () => new { message = "Healthy" });

app.Run();
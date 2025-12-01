
using UyanycarusaService.Middlewares;
using UyanycarusaService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using AspNetCoreRateLimit;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// API Versioning
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
    options.ApiVersionReader = new UrlSegmentApiVersionReader();
});

// Rate Limiting
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey is not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Swagger with JWT support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "WeBuyAnyCar USA API",
        Description = "API para consultar información de vehículos desde WeBuyAnyCar USA.",
        Contact = new OpenApiContact
        {
            Name = "WeBuyAnyCar USA Team",
            Email = "support@webuyanycarusa.com"
        }
    });

    // Include XML comments
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

    // JWT Bearer configuration for Swagger
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Health checks
builder.Services.AddHealthChecks();

// CORS Configuration
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowLocalhost", policy =>
    {
        // Lista completa de orígenes comunes de localhost para desarrollo
        var localhostOrigins = new[]
        {
            "http://localhost:3000", "https://localhost:3000",
        };

        policy.WithOrigins(localhostOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

// Services
builder.Services.AddScoped<IVehiclesService, VehiclesService>();
builder.Services.AddScoped<IValuationService, ValuationService>();
builder.Services.AddScoped<IAppointmentService, AppointmentService>();
builder.Services.AddScoped<ICustomerJourneyService, CustomerJourneyService>();
builder.Services.AddScoped<IContentService, ContentService>();
builder.Services.AddScoped<IBranchContentService, BranchContentService>();
builder.Services.AddScoped<IMakeModelContentService, MakeModelContentService>();
builder.Services.AddScoped<IAttributionService, AttributionService>();
builder.Services.AddScoped<ISchedulingService, SchedulingService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();

// HttpClient configuration for external APIs
var webuyAnyCarBaseUrl = builder.Configuration["ExternalApis:WebuyAnyCarBaseUrl"]
    ?? "https://www.webuyanycarusa.com/api";

builder.Services.AddHttpClient("WebuyAnyCarApi", client =>
{
    client.BaseAddress = new Uri(webuyAnyCarBaseUrl);
    client.Timeout = TimeSpan.FromSeconds(30);
    client.DefaultRequestHeaders.Add("Accept", "application/json");
    client.DefaultRequestHeaders.Add("User-Agent", "UyanycarusaService/1.0");
});

var app = builder.Build();

// HTTPS Enforcement
// En Development: HTTPS es recomendado pero no forzado
// En Production: HTTPS es obligatorio
if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection(); // Redirige a HTTPS pero no bloquea HTTP
}
else
{
    // En producción, forzar HTTPS
    app.UseHttpsRedirection();
    app.Use(async (context, next) =>
    {
        if (!context.Request.IsHttps)
        {
            context.Response.StatusCode = 403;
            await context.Response.WriteAsync("HTTPS is required.");
            return;
        }
        await next();
    });
}

// CORS Middleware (must be before other middlewares)
app.UseCors("AllowLocalhost");

// Rate Limiting Middleware (must be before other middlewares)
app.UseIpRateLimiting();

// Middlewares
app.UseMiddleware<ErrorHandlingMiddleware>();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "WeBuyAnyCar USA API v1");
        options.RoutePrefix = "swagger";
        options.DisplayRequestDuration();
    });
}

app.MapControllers();

// Liveness/Health
app.MapHealthChecks("/health");

app.Run();

// For integration testing
public partial class Program { }

using UyanycarusaService.Middlewares;
using UyanycarusaService.Services;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using System.IO;

var builder = WebApplication.CreateBuilder(args);

// Controllers
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

// Authorization (sin política global forzando auth en todo)
builder.Services.AddAuthorization();

// Swagger with JWT support
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "WeBuyAnyCar USA API",
        Description = "API para consultar información de vehículos desde WeBuyAnyCar USA."
    });

    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }

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
        var localhostOrigins = new[]
        {
            "http://localhost:3000", "https://localhost:3000",
            "http://localhost:3001", "https://localhost:3001",
        };

        policy.WithOrigins(localhostOrigins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });

    options.AddPolicy("AllowProduction", policy =>
    {
        var productionOrigins = new[]
        {
            "https://sellyourcarrnow.com",
            "https://www.sellyourcarrnow.com",
            "http://sellyourcarrnow.com",
            "http://www.sellyourcarrnow.com"
        };

        policy.WithOrigins(productionOrigins)
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
builder.Services.AddScoped<ISmsService, SmsService>();

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

// Forwarded headers (for NGINX reverse proxy)
// No enforce HTTPS aquí, solo para que otros middlewares puedan usarlo si quieren.
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto
});

// CORS Middleware
if (app.Environment.IsProduction())
{
    app.UseCors("AllowProduction");
}
else
{
    app.UseCors("AllowLocalhost");
}

// Rate Limiting Middleware
app.UseIpRateLimiting();

// Middlewares
app.UseMiddleware<ErrorHandlingMiddleware>();

// Authentication & Authorization
app.UseAuthentication();
app.UseAuthorization();

// Swagger (solo en desarrollo)
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

// Rutas de test / health públicas
app.MapGet("/", () => "API OK").AllowAnonymous();
app.MapGet("/health", () => "HEALTH OK").AllowAnonymous();
app.MapGet("/testalive", () => "ALIVE").AllowAnonymous();

// Controllers
app.MapControllers();

// Liveness/Health endpoint formal
app.MapHealthChecks("/healthz");

app.Run();

// For integration testing
public partial class Program { }

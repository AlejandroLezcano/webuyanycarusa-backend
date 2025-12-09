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

// Force URLs for dev mode
builder.WebHost.UseUrls("http://localhost:5000", "https://localhost:5001");

// ALWAYS lowercase URLs
builder.Services.AddRouting(o => o.LowercaseUrls = true);

// Controllers
builder.Services.AddControllers();

// API Versioning
builder.Services.AddApiVersioning(o =>
{
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.ReportApiVersions = true;
    o.ApiVersionReader = new UrlSegmentApiVersionReader();
});

// Rate Limiting
builder.Services.AddMemoryCache();
builder.Services.Configure<IpRateLimitOptions>(builder.Configuration.GetSection("IpRateLimiting"));
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();

// JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? throw new InvalidOperationException("JWT SecretKey missing");

builder.Services.AddAuthentication(o =>
{
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(o =>
{
    o.TokenValidationParameters = new TokenValidationParameters
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

builder.Services.AddAuthorization(o =>
{
    o.DefaultPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});

// Swagger + JWT fix
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "WeBuyAnyCar USA API",
        Description = "API for WBAC USA"
    });

    // Correct JWT config
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        Description = "Enter: Bearer {token}"
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

// Health Checks
builder.Services.AddHealthChecks();

// CORS
builder.Services.AddCors(o =>
{
    o.AddPolicy("AllowLocalhost", p =>
    {
        p.WithOrigins(
            "http://localhost:3000", "https://localhost:3000",
            "http://127.0.0.1:3000"
        )
        .AllowAnyHeader()
        .AllowAnyMethod()
        .AllowCredentials()
        .WithExposedHeaders("Authorization");
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

// HttpClient
var webuyBaseUrl = builder.Configuration["ExternalApis:WebuyAnyCarBaseUrl"]
    ?? "https://www.webuyanycarusa.com/api";

builder.Services.AddHttpClient("WebuyAnyCarApi", c =>
{
    c.BaseAddress = new Uri(webuyBaseUrl);
    c.Timeout = TimeSpan.FromSeconds(30);
    c.DefaultRequestHeaders.Add("Accept", "application/json");
});

// Build
var app = builder.Build();

// HTTPS
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}

// CORS BEFORE AUTH
app.UseCors("AllowLocalhost");

// Rate limiting
app.UseIpRateLimiting();

// Middlewares
app.UseMiddleware<ErrorHandlingMiddleware>();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(o =>
    {
        o.RoutePrefix = "swagger";
        o.SwaggerEndpoint("/swagger/v1/swagger.json", "WBAC API v1");
    });
}

app.MapControllers();
app.MapHealthChecks("/health");

app.Run();

public partial class Program { }

# WeBuyAnyCar USA - Backend API

REST API developed in .NET 8.0 that acts as an intermediary to consume and expose WeBuyAnyCar USA services. This API provides a centralized and secure access point to query vehicle information, perform valuations, manage appointments, and access platform-related content.

## 📋 Project Description

This project is a backend API that connects with the external WeBuyAnyCar USA API (`https://staging-api.wbac.dev`) to provide functionalities related to:

- **Vehicle Management**: Query available years, makes, and models
- **Valuations**: Vehicle value calculation
- **Appointments**: Appointment management for vehicle evaluation
- **Content**: Branch, make, and model content management
- **Customer Journey**: Customer journey tracking
- **Scheduling**: Service scheduling
- **Attribution**: Conversion and referral attribution

## ✨ Main Features

- 🔐 **JWT Authentication**: JWT token-based authentication system
- 🛡️ **Rate Limiting**: IP-based request limit control to prevent abuse
- 📚 **API Versioning**: Support for API versioning (v1, v2, etc.)
- 📖 **Swagger/OpenAPI**: Interactive API documentation available in development mode
- 🏥 **Health Checks**: Health endpoint for monitoring
- 🔒 **HTTPS Enforcement**: Forced secure connections in production
- ⚡ **Error Handling**: Centralized middleware for error handling
- 🐳 **Docker Support**: Docker container-ready configuration

## 🛠️ Technologies Used

- **.NET 8.0**: Main framework
- **ASP.NET Core Web API**: For REST API construction
- **JWT Bearer Authentication**: Token-based authentication
- **AspNetCoreRateLimit**: Request limit control
- **Swashbuckle (Swagger)**: API documentation
- **Microsoft.AspNetCore.Mvc.Versioning**: API versioning

## 📦 Prerequisites

Before starting, make sure you have installed:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) or higher
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (recommended) or [Visual Studio Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/) (optional, to clone the repository)
- [Docker](https://www.docker.com/) (optional, to run in container)

## 🚀 Installation and Configuration

### Step 1: Clone or Navigate to the Project

If you have the project in a Git repository:
```bash
git clone <repository-url>
cd buy-cars/we-buy-any-car-back
```

Or simply navigate to the project folder:
```bash
cd we-buy-any-car-back
```

### Step 2: Restore Dependencies

Navigate to the service folder and restore NuGet packages:

```bash
cd UyanycarusaService
dotnet restore
```

This command will download all necessary dependencies defined in the `UyanycarusaService.csproj` file.

### Step 3: Configure appsettings.json

The `appsettings.json` file already contains a basic configuration, but you can adjust it according to your needs:

**JWT Configuration:**
```json
"JwtSettings": {
  "SecretKey": "SuperSecretKeyCompuGlobalHyperMegaNet",  // ⚠️ Change in production
  "Issuer": "UyanycarusaService",
  "Audience": "UyanycarusaServiceUsers",
  "ExpirationInMinutes": 60
}
```

**Rate Limiting Configuration:**
```json
"IpRateLimiting": {
  "EnableEndpointRateLimiting": true,
  "GeneralRules": [
    {
      "Endpoint": "*",
      "Period": "1m",
      "Limit": 60  // 60 requests per minute
    }
  ]
}
```

**External API Configuration:**
```json
"ExternalApis": {
  "WebuyAnyCarBaseUrl": "https://staging-api.wbac.dev"
}
```

> ⚠️ **Important**: In production, change the JWT `SecretKey` to a secure and random key.

### Step 4: Verify Configuration

Make sure the `appsettings.json` file exists at:
```
UyanycarusaService/appsettings.json
```

## ▶️ How to Run the Project

### Option 1: Run from Visual Studio

1. Open the project in Visual Studio 2022
2. Select the `UyanycarusaService` profile in the toolbar
3. Press `F5` or click the "Run" button
4. The browser will automatically open at `http://localhost:5001/swagger`

### Option 2: Run from Terminal/CMD

1. Open a terminal in the project folder:
```bash
cd UyanycarusaService
```

2. Run the project:
```bash
dotnet run
```

3. The server will start and you'll see a message similar to:
```
Now listening on: http://localhost:5001
```

4. Open your browser and navigate to:
   - **Swagger UI**: `http://localhost:5001/swagger`
   - **Health Check**: `http://localhost:5001/health`

### Option 3: Run with Docker

1. From the backend project root (`we-buy-any-car-back`), build the image:
```bash
docker build -t uyanycarusa-service -f UyanycarusaService/Dockerfile .
```

2. Run the container:
```bash
docker run -p 8080:8080 uyanycarusa-service
```

3. The API will be available at: `http://localhost:8080`

## 🔑 Authentication

Most endpoints require JWT authentication. To obtain a token:

1. **Get JWT Token:**
   ```bash
   POST http://localhost:5001/api/v1/auth/login
   Content-Type: application/json
   
   {
     "username": "admin",
     "password": "password123"
   }
   ```

2. **Use the Token:**
   Include the token in the `Authorization` header of your requests:
   ```
   Authorization: Bearer <your-jwt-token>
   ```

3. **In Swagger UI:**
   - Click the "Authorize" button 🔒
   - Enter: `Bearer <your-jwt-token>`
   - Click "Authorize"

> **Note**: Currently, the login endpoint accepts any credentials. In production, this should be validated against a database or authentication service.

## 📡 Main Endpoints

### Authentication
- `POST /api/v1/auth/login` - Get JWT token (public)

### Vehicles
- `GET /api/v1/vehicles/years` - Get available years (requires authentication)
- `GET /api/v1/vehicles/makes/{year}` - Get makes by year (requires authentication)
- `GET /api/v1/vehicles/models/{year}/{make}` - Get models by year and make (requires authentication)

### Valuations
- `POST /api/v1/valuation` - Create a valuation (requires authentication)

### Appointments
- `POST /api/v1/appointment` - Create an appointment (requires authentication)

### Other Endpoints
- `GET /health` - Health check (public)
- `GET /swagger` - Swagger documentation (development only)

To see all available endpoints, visit `/swagger` when the project is running.

## 🧪 Testing

The project includes a test project in `UyanycarusaService.Tests`. To run the tests:

```bash
cd UyanycarusaService.Tests
dotnet test
```

## 📁 Project Structure

```
we-buy-any-car-back/
├── UyanycarusaService/
│   ├── Controllers/          # API controllers
│   ├── Services/             # Business logic
│   │   └── Interfaces/       # Service interfaces
│   ├── Dtos/                 # Data Transfer Objects
│   ├── Middlewares/          # Custom middlewares
│   ├── ModelsTests/          # Test data
│   ├── Configuration/        # Configurations
│   ├── Properties/           # Launch configuration
│   ├── Program.cs            # Entry point
│   ├── appsettings.json      # Application configuration
│   └── UyanycarusaService.csproj
├── UyanycarusaService.Tests/ # Test project
└── README.md                 # This file
```

## 🔧 Environment Configuration

The project supports different environments through environment variables:

- **Development**: `ASPNETCORE_ENVIRONMENT=Development`
- **Production**: `ASPNETCORE_ENVIRONMENT=Production`

In development, Swagger is enabled. In production, HTTPS is mandatory.

## 🐛 Troubleshooting

### Error: "JWT SecretKey is not configured"
- Verify that the `appsettings.json` file contains the `JwtSettings` section with `SecretKey`

### Error: "Cannot connect to external API"
- Verify that the URL in `ExternalApis:WebuyAnyCarBaseUrl` is correct
- Check your internet connection
- Review the logs for more error details

### Port already in use
- Change the port in `Properties/launchSettings.json` or use:
```bash
dotnet run --urls "http://localhost:5002"
```

## 📝 Additional Notes

- The `bin/` and `obj/` folders can be safely deleted. They are automatically regenerated when compiling.
- The project uses test data when `dataTest: true` is set in `appsettings.json`
- Logs are configured in `appsettings.json` under the `Logging` section

## 📄 License

This project is private and for internal use.

## 👥 Contributors

WeBuyAnyCar USA Development Team

---

**Need help?** Check the Swagger documentation at `/swagger` or contact the development team.

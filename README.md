# WeBuyAnyCar USA - Backend API

API REST desarrollada en .NET 8.0 que actúa como intermediario para consumir y exponer servicios de WeBuyAnyCar USA. Esta API proporciona un punto de acceso centralizado y seguro para consultar información sobre vehículos, realizar valuaciones, gestionar citas y acceder a contenido relacionado con la plataforma.

## 📋 Descripción del Proyecto

Este proyecto es una API backend que se conecta con la API externa de WeBuyAnyCar USA (`https://staging-api.wbac.dev`) para proporcionar funcionalidades relacionadas con:

- **Gestión de Vehículos**: Consulta de años, marcas y modelos disponibles
- **Valuaciones**: Cálculo de valor de vehículos
- **Citas (Appointments)**: Gestión de citas para evaluación de vehículos
- **Contenido**: Gestión de contenido de sucursales, marcas y modelos
- **Customer Journey**: Seguimiento del recorrido del cliente
- **Scheduling**: Programación de servicios
- **Attribution**: Atribución de conversiones y referencias

## ✨ Características Principales

- 🔐 **Autenticación JWT**: Sistema de autenticación basado en tokens JWT
- 🛡️ **Rate Limiting**: Control de límites de solicitudes por IP para prevenir abusos
- 📚 **API Versioning**: Soporte para versionado de API (v1, v2, etc.)
- 📖 **Swagger/OpenAPI**: Documentación interactiva de la API disponible en modo desarrollo
- 🏥 **Health Checks**: Endpoint de salud para monitoreo
- 🔒 **HTTPS Enforcement**: Forzado de conexiones seguras en producción
- ⚡ **Error Handling**: Middleware centralizado para manejo de errores
- 🐳 **Docker Support**: Configuración lista para contenedores Docker

## 🛠️ Tecnologías Utilizadas

- **.NET 8.0**: Framework principal
- **ASP.NET Core Web API**: Para construcción de la API REST
- **JWT Bearer Authentication**: Autenticación basada en tokens
- **AspNetCoreRateLimit**: Control de límites de solicitudes
- **Swashbuckle (Swagger)**: Documentación de API
- **Microsoft.AspNetCore.Mvc.Versioning**: Versionado de API

## 📦 Requisitos Previos

Antes de comenzar, asegúrate de tener instalado:

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) o superior
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (recomendado) o [Visual Studio Code](https://code.visualstudio.com/)
- [Git](https://git-scm.com/) (opcional, para clonar el repositorio)
- [Docker](https://www.docker.com/) (opcional, para ejecutar en contenedor)

## 🚀 Instalación y Configuración

### Paso 1: Clonar o Navegar al Proyecto

Si tienes el proyecto en un repositorio Git:
```bash
git clone <url-del-repositorio>
cd buy-cars/we-buy-any-car-back
```

O simplemente navega a la carpeta del proyecto:
```bash
cd we-buy-any-car-back
```

### Paso 2: Restaurar Dependencias

Navega a la carpeta del servicio y restaura los paquetes NuGet:

```bash
cd UyanycarusaService
dotnet restore
```

Este comando descargará todas las dependencias necesarias definidas en el archivo `UyanycarusaService.csproj`.

### Paso 3: Configurar appsettings.json

El archivo `appsettings.json` ya contiene una configuración básica, pero puedes ajustarla según tus necesidades:

**Configuración de JWT:**
```json
"JwtSettings": {
  "SecretKey": "SuperSecretKeyCompuGlobalHyperMegaNet",  // ⚠️ Cambiar en producción
  "Issuer": "UyanycarusaService",
  "Audience": "UyanycarusaServiceUsers",
  "ExpirationInMinutes": 60
}
```

**Configuración de Rate Limiting:**
```json
"IpRateLimiting": {
  "EnableEndpointRateLimiting": true,
  "GeneralRules": [
    {
      "Endpoint": "*",
      "Period": "1m",
      "Limit": 60  // 60 solicitudes por minuto
    }
  ]
}
```

**Configuración de API Externa:**
```json
"ExternalApis": {
  "WebuyAnyCarBaseUrl": "https://staging-api.wbac.dev"
}
```

> ⚠️ **Importante**: En producción, cambia el `SecretKey` del JWT por una clave segura y aleatoria.

### Paso 4: Verificar la Configuración

Asegúrate de que el archivo `appsettings.json` existe en la ruta:
```
UyanycarusaService/appsettings.json
```

## ▶️ Cómo Ejecutar el Proyecto

### Opción 1: Ejecutar desde Visual Studio

1. Abre el proyecto en Visual Studio 2022
2. Selecciona el perfil `UyanycarusaService` en la barra de herramientas
3. Presiona `F5` o haz clic en el botón "Ejecutar"
4. El navegador se abrirá automáticamente en `http://localhost:5001/swagger`

### Opción 2: Ejecutar desde la Terminal/CMD

1. Abre una terminal en la carpeta del proyecto:
```bash
cd UyanycarusaService
```

2. Ejecuta el proyecto:
```bash
dotnet run
```

3. El servidor se iniciará y verás un mensaje similar a:
```
Now listening on: http://localhost:5001
```

4. Abre tu navegador y navega a:
   - **Swagger UI**: `http://localhost:5001/swagger`
   - **Health Check**: `http://localhost:5001/health`

### Opción 3: Ejecutar con Docker

1. Desde la raíz del proyecto backend (`we-buy-any-car-back`), construye la imagen:
```bash
docker build -t uyanycarusa-service -f UyanycarusaService/Dockerfile .
```

2. Ejecuta el contenedor:
```bash
docker run -p 8080:8080 uyanycarusa-service
```

3. La API estará disponible en: `http://localhost:8080`

## 🔑 Autenticación

La mayoría de los endpoints requieren autenticación JWT. Para obtener un token:

1. **Obtener Token JWT:**
   ```bash
   POST http://localhost:5001/api/v1/auth/login
   Content-Type: application/json
   
   {
     "username": "admin",
     "password": "password123"
   }
   ```

2. **Usar el Token:**
   Incluye el token en el header `Authorization` de tus solicitudes:
   ```
   Authorization: Bearer <tu-token-jwt>
   ```

3. **En Swagger UI:**
   - Haz clic en el botón "Authorize" 🔒
   - Ingresa: `Bearer <tu-token-jwt>`
   - Haz clic en "Authorize"

> **Nota**: Actualmente, el endpoint de login acepta cualquier credencial. En producción, esto debe validarse contra una base de datos o servicio de autenticación.

## 📡 Endpoints Principales

### Autenticación
- `POST /api/v1/auth/login` - Obtener token JWT (público)

### Vehículos
- `GET /api/v1/vehicles/years` - Obtener años disponibles (requiere autenticación)
- `GET /api/v1/vehicles/makes/{year}` - Obtener marcas por año (requiere autenticación)
- `GET /api/v1/vehicles/models/{year}/{make}` - Obtener modelos por año y marca (requiere autenticación)

### Valuaciones
- `POST /api/v1/valuation` - Crear una valuación (requiere autenticación)

### Citas
- `POST /api/v1/appointment` - Crear una cita (requiere autenticación)

### Otros Endpoints
- `GET /health` - Health check (público)
- `GET /swagger` - Documentación Swagger (solo en desarrollo)

Para ver todos los endpoints disponibles, visita `/swagger` cuando el proyecto esté en ejecución.

## 🧪 Testing

El proyecto incluye un proyecto de pruebas en `UyanycarusaService.Tests`. Para ejecutar las pruebas:

```bash
cd UyanycarusaService.Tests
dotnet test
```

## 📁 Estructura del Proyecto

```
we-buy-any-car-back/
├── UyanycarusaService/
│   ├── Controllers/          # Controladores de la API
│   ├── Services/             # Lógica de negocio
│   │   └── Interfaces/       # Interfaces de servicios
│   ├── Dtos/                 # Data Transfer Objects
│   ├── Middlewares/          # Middlewares personalizados
│   ├── ModelsTests/          # Datos de prueba
│   ├── Configuration/        # Configuraciones
│   ├── Properties/           # Configuración de lanzamiento
│   ├── Program.cs            # Punto de entrada
│   ├── appsettings.json      # Configuración de la aplicación
│   └── UyanycarusaService.csproj
├── UyanycarusaService.Tests/ # Proyecto de pruebas
└── README.md                 # Este archivo
```

## 🔧 Configuración de Entornos

El proyecto soporta diferentes entornos mediante variables de entorno:

- **Development**: `ASPNETCORE_ENVIRONMENT=Development`
- **Production**: `ASPNETCORE_ENVIRONMENT=Production`

En desarrollo, Swagger está habilitado. En producción, HTTPS es obligatorio.

## 🐛 Solución de Problemas

### Error: "JWT SecretKey is not configured"
- Verifica que el archivo `appsettings.json` contenga la sección `JwtSettings` con `SecretKey`

### Error: "Cannot connect to external API"
- Verifica que la URL en `ExternalApis:WebuyAnyCarBaseUrl` sea correcta
- Verifica tu conexión a internet
- Revisa los logs para más detalles del error

### Puerto ya en uso
- Cambia el puerto en `Properties/launchSettings.json` o usa:
```bash
dotnet run --urls "http://localhost:5002"
```

## 📝 Notas Adicionales

- La carpeta `bin/` y `obj/` pueden eliminarse de forma segura. Se regeneran automáticamente al compilar.
- El proyecto usa datos de prueba cuando `dataTest: true` está en `appsettings.json`
- Los logs se configuran en `appsettings.json` bajo la sección `Logging`

## 📄 Licencia

Este proyecto es privado y de uso interno.

## 👥 Contribuidores

Equipo de desarrollo WeBuyAnyCar USA

---

**¿Necesitas ayuda?** Revisa la documentación de Swagger en `/swagger` o contacta al equipo de desarrollo.

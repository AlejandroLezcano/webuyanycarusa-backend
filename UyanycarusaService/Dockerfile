
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY UyanycarusaService/UyanycarusaService.csproj UyanycarusaService/
RUN dotnet restore UyanycarusaService/UyanycarusaService.csproj
COPY . .
RUN dotnet publish UyanycarusaService/UyanycarusaService.csproj -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 8080
# HTTP (Optional) also expose 8081 if needed
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UyanycarusaService.dll"]

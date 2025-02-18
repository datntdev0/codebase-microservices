FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

WORKDIR /src
COPY ["src/datntdev.Microservice.Web.Host/datntdev.Microservice.Web.Host.csproj", "src/datntdev.Microservice.Web.Host/"]
COPY ["src/datntdev.Microservice.Web.Core/datntdev.Microservice.Web.Core.csproj", "src/datntdev.Microservice.Web.Core/"]
COPY ["src/datntdev.Microservice.Application/datntdev.Microservice.Application.csproj", "src/datntdev.Microservice.Application/"]
COPY ["src/datntdev.Microservice.Core/datntdev.Microservice.Core.csproj", "src/datntdev.Microservice.Core/"]
COPY ["src/datntdev.Microservice.EntityFrameworkCore/datntdev.Microservice.EntityFrameworkCore.csproj", "src/datntdev.Microservice.EntityFrameworkCore/"]
WORKDIR "/src/src/datntdev.Microservice.Web.Host"
RUN dotnet restore 

WORKDIR /src
COPY ["src/datntdev.Microservice.Web.Host", "src/datntdev.Microservice.Web.Host"]
COPY ["src/datntdev.Microservice.Web.Core", "src/datntdev.Microservice.Web.Core"]
COPY ["src/datntdev.Microservice.Application", "src/datntdev.Microservice.Application"]
COPY ["src/datntdev.Microservice.Core", "src/datntdev.Microservice.Core"]
COPY ["src/datntdev.Microservice.EntityFrameworkCore", "src/datntdev.Microservice.EntityFrameworkCore"]
WORKDIR "/src/src/datntdev.Microservice.Web.Host"
RUN dotnet publish -c Release -o /publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0
EXPOSE 80
WORKDIR /app
COPY --from=build /publish .
ENTRYPOINT ["dotnet", "datntdev.Microservice.Web.Host.dll"]

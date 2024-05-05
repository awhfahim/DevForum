FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443
 
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StackOverflow.Web/StackOverflow.Web.csproj", "StackOverflow.Web/"]
COPY ["StackOverflow.Application/StackOverflow.Application.csproj", "StackOverflow.Application/"]
COPY ["StackOverflow.Application.Contracts/StackOverflow.Application.Contracts.csproj", "StackOverflow.Application.Contracts/"]
COPY ["StackOverflow.Domain/StackOverflow.Domain.csproj", "StackOverflow.Domain/"]
COPY ["StackOverflow.Infrastructure/StackOverflow.Infrastructure.csproj", "StackOverflow.Infrastructure/"]
RUN dotnet restore "./StackOverflow.Web/StackOverflow.Web.csproj"
COPY . .
WORKDIR "/src/StackOverflow.Web"
RUN dotnet build "./StackOverflow.Web.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./StackOverflow.Web.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StackOverflow.Web.dll"]
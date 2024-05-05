FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["StackOverflow.Service/StackOverflow.Service.csproj", "StackOverflow.Service/"]
COPY ["StackOverflow.Application/StackOverflow.Application.csproj", "StackOverflow.Application/"]
COPY ["StackOverflow.Application.Contracts/StackOverflow.Application.Contracts.csproj", "StackOverflow.Application.Contracts/"]
COPY ["StackOverflow.Domain/StackOverflow.Domain.csproj", "StackOverflow.Domain/"]
COPY ["StackOverflow.Infrastructure/StackOverflow.Infrastructure.csproj", "StackOverflow.Infrastructure/"]
RUN dotnet restore "StackOverflow.Service/StackOverflow.Service.csproj"
COPY . .
WORKDIR "/src/StackOverflow.Service"
RUN dotnet build "StackOverflow.Service.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "StackOverflow.Service.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StackOverflow.Service.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Authentication/Authentication.API/Authentication.API.csproj", "Services/Authentication/Authentication.API/"]
COPY ["Services/Authentication/Authentication.DataLayer/Authentication.DataLayer.csproj", "Services/Authentication/Authentication.DataLayer/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Services/Authentication/Authentication.BusinessLogic/Authentication.BusinessLogic.csproj", "Services/Authentication/Authentication.BusinessLogic/"]
RUN dotnet restore "Services/Authentication/Authentication.API/Authentication.API.csproj"
COPY ./Services/Authentication/ ./Services/Authentication
COPY ./Shared/ ./Shared
WORKDIR "/src/Services/Authentication/Authentication.API"
RUN dotnet build "Authentication.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Authentication.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Authentication.API.dll"]

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ApiGateway.Ocelot/ApiGateway.Ocelot.csproj", "ApiGateway.Ocelot/"]
RUN dotnet restore "ApiGateway.Ocelot/ApiGateway.Ocelot.csproj"
COPY . .
WORKDIR "/src/ApiGateway.Ocelot"
RUN dotnet build "ApiGateway.Ocelot.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ApiGateway.Ocelot.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ApiGateway.Ocelot.dll", "--launch-profile Docker"]

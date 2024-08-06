FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Match/Match.Presentation/Match.Presentation.csproj", "Services/Match/Match.Presentation/"]
COPY ["Shared/Shared.csproj", "Shared/"]
COPY ["Services/Match/Match.Domain/Match.Domain.csproj", "Services/Match/Match.Domain/"]
COPY ["Services/Match/Match.Application/Match.Application.csproj", "Services/Match/Match.Application/"]
COPY ["Services/Match/Match.Infrastructure/Match.Infrastructure.csproj", "Services/Match/Match.Infrastructure/"]
RUN dotnet restore "src/Services/Match/Match.Presentation/Match.Presentation.csproj"
COPY ./Services/Match/ ./Services/Match
COPY ./Shared/ ./Shared
WORKDIR "/src/src/Services/Match/Match.Presentation"
RUN dotnet build "Match.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Match.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Match.Presentation.dll", "--launch-profile Docker"]

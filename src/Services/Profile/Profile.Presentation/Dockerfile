﻿FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 443
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Services/Profile/Profile.Presentation/Profile.Presentation.csproj", "Services/Profile/Profile.Presentation/"]
COPY ["Services/Profile/Profile.Infrastructure/Profile.Infrastructure.csproj", "Services/Profile/Profile.Infrastructure/"]
COPY ["Services/Profile/Profile.Application/Profile.Application.csproj", "Services/Profile/Profile.Application/"]
COPY ["Services/Profile/Profile.Domain/Profile.Domain.csproj", "Services/Profile/Profile.Domain/"]
COPY ["Shared/Shared.csproj", "Shared/"]
RUN dotnet restore "src/Services/Profile/Profile.Presentation/Profile.Presentation.csproj"
COPY . .
WORKDIR "/src/src/Services/Profile/Profile.Presentation"
RUN dotnet build "Profile.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Profile.Presentation.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Profile.Presentation.dll", "--launch-profile Docker"]

FROM  mcr.microsoft.com/dotnet/aspnet:8.0-noble-chiseled AS base
USER $APP_UID
WORKDIR /app
LABEL maintainer="tyrongower"
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["uptime_robot_exporter.csproj", "./"]
RUN dotnet restore "uptime_robot_exporter.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "uptime_robot_exporter.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "uptime_robot_exporter.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_ENVIRONMENT=Production
ENV DOTNET_USE_POLLING_FILE_WATCHER=true
ENV PATH=/usr/local/sbin:/usr/local/bin:/usr/sbin:/usr/bin:/sbin:/bin
ENTRYPOINT ["dotnet", "/app/uptime_robot_exporter.dll"]

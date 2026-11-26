FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_HTTP_PORTS=8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["HealthCare.PL/HealthCare.PL.csproj", "HealthCare.PL/"]
COPY ["HealthCare.BLL/HealthCare.BLL.csproj", "HealthCare.BLL/"]
COPY ["HealthCare.DAL/HealthCare.DAL.csproj", "HealthCare.DAL/"]
RUN dotnet restore "HealthCare.PL/HealthCare.PL.csproj"
COPY . .
RUN dotnet publish "HealthCare.PL/HealthCare.PL.csproj" --configuration Release --no-restore --output /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "HealthCare.PL.dll"]

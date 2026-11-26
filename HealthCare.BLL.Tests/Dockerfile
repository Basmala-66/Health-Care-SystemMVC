FROM mcr.microsoft.com/dotnet/sdk:8.0
WORKDIR /src

COPY ["HealthCareSystem.sln", "./"]
COPY ["HealthCare.BLL.Tests/HealthCare.BLL.Tests.csproj", "HealthCare.BLL.Tests/"]
COPY ["HealthCare.BLL/HealthCare.BLL.csproj", "HealthCare.BLL/"]
COPY ["HealthCare.DAL/HealthCare.DAL.csproj", "HealthCare.DAL/"]
RUN dotnet restore "HealthCare.BLL.Tests/HealthCare.BLL.Tests.csproj"

COPY . .
ENTRYPOINT ["dotnet", "test", "HealthCare.BLL.Tests/HealthCare.BLL.Tests.csproj", "--configuration", "Release", "--no-restore"]

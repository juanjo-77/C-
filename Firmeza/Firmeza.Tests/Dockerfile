FROM mcr.microsoft.com/dotnet/sdk:9.0 AS test
WORKDIR /src

COPY ["Firmeza.Tests/Firmeza.Tests.csproj", "Firmeza.Tests/"]
COPY ["Firmeza.Web/Firmeza.Web.csproj", "Firmeza.Web/"]
COPY ["Firmeza.API/Firmeza.API.csproj", "Firmeza.API/"]

RUN dotnet restore "Firmeza.Tests/Firmeza.Tests.csproj"

COPY . .

RUN dotnet build "Firmeza.Tests/Firmeza.Tests.csproj" -c Release

ENTRYPOINT ["dotnet", "test", "Firmeza.Tests/Firmeza.Tests.csproj", "--no-build", "-c", "Release", "--logger", "console;verbosity=normal"]
#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["Employee.API/Employee.API.csproj", "Employee.API/"]
COPY ["Employee.Interfaces/Employee.Interfaces.csproj", "Employee.Interfaces/"]
COPY ["Employee.SQL.Infrastructure/Employee.SQL.Infrastructure.csproj", "Employee.SQL.Infrastructure/"]
COPY ["Employee.Domain/Employee.Domain.csproj", "Employee.Domain/"]
COPY ["Employee.Services/Employee.Services.csproj", "Employee.Services/"]
RUN dotnet restore "Employee.API/Employee.API.csproj"
COPY . .
WORKDIR "/src/Employee.API"
RUN dotnet build "Employee.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Employee.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Employee.API.dll"]
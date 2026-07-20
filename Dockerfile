# Sử dụng image .NET 10.0 SDK để build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /app

# Copy project file và restore
COPY *.csproj ./
RUN dotnet restore

# Copy toàn bộ code và build
COPY . ./
RUN dotnet publish -c Release -o out

# Sử dụng image .NET 10.0 Runtime để chạy (nhẹ hơn)
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app/out .

# Mở port
EXPOSE 8080

# Chạy ứng dụng
ENTRYPOINT ["dotnet", "LanguageLearningApp.dll"]

# 1. GIAI ĐOẠN BUILD (Dùng bộ SDK .NET 10 mới nhất)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy file dự án
COPY ["MangakaApp.csproj", "./"]
# Tải các thư viện
RUN dotnet restore "MangakaApp.csproj"

# Copy toàn bộ code
COPY . .
WORKDIR "/src/."
# Build ra bản Release
RUN dotnet build "MangakaApp.csproj" -c Release -o /app/build

# Xuất bản (Publish) RUN dotnet publish: Là lệnh đóng gói code lại (giống như khi  bấm Publish trong Visual Studio ).
FROM build AS publish
RUN dotnet publish "MangakaApp.csproj" -c Release -o /app/publish

# 2. GIAI ĐOẠN CHẠY (Dùng môi trường ASP.NET 10 để chạy cho nhẹ) Là lệnh bảo máy chủ tải bộ công cụ .NET 10 về. Là lệnh copy code từ máy local vào máy chủ.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=publish /app/publish . 

# Mở cổng 8080 (Render yêu cầu cổng này)
ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

# Chạy file MangakaApp.dll
ENTRYPOINT ["dotnet", "MangakaApp.dll"]
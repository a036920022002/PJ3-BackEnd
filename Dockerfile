# 第一階段：編譯環境 (使用 .NET 10 SDK)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 1. 複製專案檔並還原套件
COPY ["PJ3-BackEnd/PJ3-BackEnd.csproj", "PJ3-BackEnd/"]
RUN dotnet restore "PJ3-BackEnd/PJ3-BackEnd.csproj"

# 2. 複製剩餘檔案
COPY . .

# 3. 執行發佈
WORKDIR "/src/PJ3-BackEnd"
RUN dotnet publish "PJ3-BackEnd.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 第二階段：執行環境 (必須也使用 .NET 10 Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# 設定 Zeabur 要求的環境變數
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "PJ3-BackEnd.dll"]
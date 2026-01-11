# 使用 .NET 8 SDK 進行編譯
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# 複製專案檔並進行還原
# 根據你的 GitHub 截圖，PJ3-BackEnd 資料夾在根目錄下
COPY ["PJ3-BackEnd/PJ3-BackEnd.csproj", "PJ3-BackEnd/"]
RUN dotnet restore "PJ3-BackEnd/PJ3-BackEnd.csproj"

# 複製其餘原始碼
COPY . .
WORKDIR "/src/PJ3-BackEnd"

# 發佈專案
RUN dotnet publish "PJ3-BackEnd.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 建立執行階段映像
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# 設定 Zeabur 要求的環境變數
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# 啟動 API
ENTRYPOINT ["dotnet", "PJ3-BackEnd.dll"]
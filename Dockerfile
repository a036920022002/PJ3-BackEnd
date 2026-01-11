# 第一階段：編譯 (使用 sdk:10.0 並命名為 build_env)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build_env
WORKDIR /src

# 1. 複製專案檔並還原
COPY ["PJ3-BackEnd/PJ3-BackEnd.csproj", "PJ3-BackEnd/"]
RUN dotnet restore "PJ3-BackEnd/PJ3-BackEnd.csproj"

# 2. 複製其餘原始碼並編譯
COPY . .
WORKDIR "/src/PJ3-BackEnd"
RUN dotnet publish "PJ3-BackEnd.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 第二階段：執行環境 (使用 aspnet:10.0)
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# 從名為 build_env 的階段複製編譯好的檔案
COPY --from=build_env /app/publish .

# 確保 public 資料夾存在，避免你的 Program.cs 第 57 行報錯
RUN mkdir -p /app/public

# 設定監聽埠號
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

ENTRYPOINT ["dotnet", "PJ3-BackEnd.dll"]
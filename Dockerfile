# 第一階段：編譯 (使用 sdk:10.0 並命名為 build_env)
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build_env
WORKDIR /src

# 1. 複製專案檔並還原
# 這裡假設您的目錄結構是：根目錄下有 PJ3-BackEnd 資料夾
COPY ["PJ3-BackEnd/PJ3-BackEnd.csproj", "PJ3-BackEnd/"]
RUN dotnet restore "PJ3-BackEnd/PJ3-BackEnd.csproj"

# 2. 複製其餘原始碼並編譯
COPY . .
WORKDIR "/src/PJ3-BackEnd"
RUN dotnet publish "PJ3-BackEnd.csproj" -c Release -o /app/publish /p:UseAppHost=false

# 第二階段：執行環境 (使用 aspnet:10.0)
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

# A. 從編譯階段複製二進位執行檔
COPY --from=build_env /app/publish .

# B. 關鍵修正：將原始碼中的 public 資料夾完整複製到執行環境
# 這樣你的圖片才會真的被打包進去
COPY --from=build_env /src/PJ3-BackEnd/public ./public

# 設定監聽埠號
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# 選擇性：預先建立 DataProtection 金鑰目錄以減少警告
RUN mkdir -p /root/.aspnet/DataProtection-Keys

ENTRYPOINT ["dotnet", "PJ3-BackEnd.dll"]
# ... 前面不變 ...
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
COPY --from=build /app/publish .

# 加上這一行：確保 public 資料夾存在
RUN mkdir -p /app/public

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
ENTRYPOINT ["dotnet", "PJ3-BackEnd.dll"]
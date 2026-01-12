using Microsoft.AspNetCore.Authentication.JwtBearer; 
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.FileProviders;
using PJ3_BackEnd.Models;
using PJ3_BackEnd.Service;
using System.Text;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions
{
    Args = args,
    WebRootPath = "public"
});

builder.Services.AddControllers();

//CORS 設定
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("myprofilebycsharp.zeabur.app") // 輸入前端地址
                                .AllowAnyHeader()
                                .AllowAnyMethod();
        });
});

//資料庫依賴注入部分
builder.Services.AddDbContext<profileContext>(options =>options.UseMySQL(builder.Configuration.GetConnectionString("WebDatabase")));
//JWT 設定部分
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidateAudience = true,
            ValidAudience = builder.Configuration["Jwt:Audience"],
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])),
            ClockSkew = TimeSpan.Zero // 建議設為 0，讓 Token 過期立即失效
        };
    });
builder.Services.AddScoped<IPasswordService, bcrypt>();

var app = builder.Build();
 
// Configure the HTTP request pipeline.

app.UseHttpsRedirection();
app.UseCors(myAllowSpecificOrigins);
app.UseStaticFiles();

// 針對 public 資料夾的對應
app.UseStaticFiles(new StaticFileOptions
{
    // 指定實體檔案路徑：專案根目錄下的 public 資料夾
    FileProvider = new PhysicalFileProvider(
        Path.Combine(builder.Environment.ContentRootPath, "public")),

    // 指定網路請求路徑：當網址出現 /public 時，對應到上面的資料夾
    RequestPath = "/public"
});


app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

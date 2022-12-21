using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Rong.Core.Models;
using Rong.Infra.Database;
using Rong.Infra.Extensions;
using Rong.Infra.Helper;
using Rong.Infra.Middlewares;
using WatchDog;
using WatchDog.src.Enums;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllers();
services.AddEndpointsApiExplorer();
services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1",
        new OpenApiInfo {Title = "rong-blog API", Version = "v1"});

    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "在下框中输入请求头中需要添加Jwt授权Token: Bearer Token",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });

    var xmlFile =
        $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath, true);
});

// 启用 Jwt token 验证
var jwtSettings = AppConfigurationHelper.GetSection<JwtSettings>(nameof(JwtSettings));
services.AddAuthentication(c =>
{
    c.DefaultAuthenticateScheme =
        JwtBearerDefaults.AuthenticationScheme;
    c.DefaultChallengeScheme =
        JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(c =>
{
    c.RequireHttpsMetadata = false;
    c.SaveToken = true;
    c.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(jwtSettings.Secret)),
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        ValidateIssuer = false,
        ValidateAudience = false
    };
});

// 启用 SqlSugar
services.AddSqlSugarSetup();
services.AddAutoDi();

// 启用 watchdog 服务
services.AddWatchDogServices(opt =>
{
    opt.IsAutoClear = true;
    opt.ClearTimeSchedule = WatchDogAutoClearScheduleEnum.Monthly;
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "rong-blog API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();

app.UseWatchDog(opt =>
{
    opt.WatchPageUsername = "admin";
    opt.WatchPagePassword = "123456";
});

app.UseMiddleware<CustomExceptionMiddleware>();

// 认证
app.UseAuthentication();
// 鉴权
app.UseAuthorization();

app.MapControllers();

app.Run();
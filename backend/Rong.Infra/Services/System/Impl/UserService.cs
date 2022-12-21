using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Rong.Core.Dtos.System;
using Rong.Core.Entities;
using Rong.Core.Enums;
using Rong.Core.Models;
using Rong.Infra.Attributes;
using Rong.Infra.Database;
using Rong.Infra.Helper;
using SqlSugar;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Rong.Infra.Services.System.Impl;

[AutoInject(typeof(IUserService), InjectType.Scoped)]
public class UserService: SugarRepository<User>, IUserService
{
    public UserService(ISqlSugarClient context): base(context)
    {
    }
    public async Task<Response> Register(Register dto)
    {
        Response res = new();
        if (await Context.Queryable<User>().AnyAsync(c => c.UserName == dto.userName))
        {
            res.Message = "该用户名已存在";
            res.Success = false;
            return res;
        }
        var user = new User()
        {
            UserName = dto.userName,
            Password = EncryptHelper.AesEncrypt(dto.password),
            Status = Status.Enabled,
            CreateTime = TimeHelper.GetTimeStamp(),
            UpdateTime = TimeHelper.GetTimeStamp()
        };

        var result = await Context.Storageable(user).ExecuteCommandAsync();
        res.Success = result > 0;
        res.Message = result > 0 ? "注册成功" : "注册失败";
        return res;
    }

    public async Task<Response> Login(Login dto)
    {
        Response res = new() { Success = true };
        var user = await Context.Queryable<User>().Where(it => it.UserName == dto.userName).FirstAsync();
        if (user == null)
        {
            res.Message = "用户名密码错误";
            res.Success = false;
            return res;
        }
        
        var password = EncryptHelper.AesEncrypt(dto.password);
        if (user.Password != password)
        {
            res.Message = "用户名密码错误";
            res.Success = false;
            return res;
        }

        if (user.Status == Status.Disabled)
        {
            res.Message = "该用户已被禁用";
            res.Success = false;
            return res;
        }

        var jwtSettings = AppConfigurationHelper.GetSection<JwtSettings>(nameof(JwtSettings));
        var claims = new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, user.Id.ToString())
        };
        
        var key =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret));
        var credentials =
            new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var jwtToken = new JwtSecurityToken(jwtSettings.Issuer, jwtSettings.Audience,
            claims,
            expires: DateTime.Now.AddMinutes(jwtSettings.AccessExpiration),
            signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(jwtToken);

        res.Message = "登录成功";
        res.Data = new
        {
            token,
            userInfo = new
            {
                user.Id,
                user.UserName
            }
        };
        return res;
    }
}
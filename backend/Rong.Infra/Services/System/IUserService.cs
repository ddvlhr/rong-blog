using Rong.Core.Dtos.System;
using Rong.Core.Models;

namespace Rong.Infra.Services.System;

public interface IUserService
{
    Task<Response> Register(Register dto);
    Task<Response> Login(Login dto);
}
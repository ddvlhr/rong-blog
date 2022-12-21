using Microsoft.AspNetCore.Mvc;
using Rong.Core.Models;

namespace Rong.Api.Controllers;

[ApiController]
public class BaseController: ControllerBase
{
    protected ActionResult<T> Success<T>(T? data, string message)
    {
        return Ok(new Response() {Success = true, Data = data, Message = message});
    }
    
    
}
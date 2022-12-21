using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Rong.Core.Models;

namespace Rong.Infra.Middlewares;

public class CustomExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<CustomExceptionMiddleware> _logger;

    public CustomExceptionMiddleware(RequestDelegate next, ILogger<CustomExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }
    
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
    
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

        var response = context.Response;
        var result = new Response()
        {
            Success = false
        };

        switch (exception)
        {
            case ApplicationException ex:
                if (ex.Message.Contains("Invalid token"))
                {
                    response.StatusCode = (int) HttpStatusCode.Forbidden;
                    result.Message = ex.Message;
                    break;
                }
                response.StatusCode = (int) HttpStatusCode.BadRequest;
                result.Message = ex.Message;
                break;
            case KeyNotFoundException ex:
                response.StatusCode = (int)HttpStatusCode.NotFound;
                result.Message = ex.Message;
                break;
            default:
                response.StatusCode = (int) HttpStatusCode.InternalServerError;
                result.Message = "服务器内部错误, 请查看详细错误日志！";
                break;
        }
        _logger.LogError("Error: {msg}", exception.Message);
        await context.Response.WriteAsync(JsonSerializer.Serialize(result));
    }
}
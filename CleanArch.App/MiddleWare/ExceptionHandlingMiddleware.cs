using CleanArch.App.Services;
using CleanArch.Common.Enums;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace CleanArch.App.MiddleWare
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
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

        private async Task HandleExceptionAsync(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            int statusCode;
            string message;

            switch (ex)
            {
                case UnauthorizedAccessException:
                    statusCode = (int)StatusCodesEnum.NotAuthorized;
                    message = "Unauthorized access";
                    break;

                case KeyNotFoundException:
                    statusCode = (int)StatusCodesEnum.Notfound;
                    message = "Resource not found";
                    break;

                case InvalidOperationException:
                    statusCode = (int)StatusCodesEnum.BadRequest;
                    message = ex.Message;
                    break;

                case DbUpdateException:
                    statusCode = (int)StatusCodesEnum.ServerError;
                    message = "Database update error";
                    break;

                case ApplicationException:
                    statusCode = (int)StatusCodesEnum.BadRequest;
                    message = ex.Message;
                    break;

                default:
                    statusCode = (int)StatusCodesEnum.ServerError;
                    message = "Something went wrong, please try again later.";
                    break;
            }

            _logger.LogError(ex, $"[ERROR] {ex.Message}");

            var response = new ResponseModel
            {
                StatusCode = statusCode,
                Timestamp = DateTime.UtcNow,
                IsError = true,
                Message = message,
                Result = new { error = ex.Message } 
            };

            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

    public static class ExceptionHandlingMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandling(this IApplicationBuilder app)
        {
            return app.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}

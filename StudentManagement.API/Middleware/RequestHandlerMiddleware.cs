

using System.Globalization;
using Microsoft.AspNetCore.Http.HttpResults;
using StudentManagement.Persistence.Models;
namespace StudentManagement.API.Middleware
{
    public class RequestHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestHandlerMiddleware> _logger;
        public RequestHandlerMiddleware(RequestDelegate next, ILogger<RequestHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            _logger.LogInformation("***SampleCustomMiddlewre is called***");
            _logger.LogInformation("Request Logging : " + context.Request.Method + " " + context.Request.Path + context.Request.QueryString);

            try
            {
               
                await _next(context);

                var statusCode = context.Response.StatusCode;
                if (statusCode >= 500)
                {
                    _logger.LogError($"Server error occurred! Status Code: {statusCode}");
                }
                else if (statusCode >= 400)
                {
                    
                    _logger.LogWarning($"Client error occurred! Status Code: {statusCode}");
                }
                else if (statusCode >= 300)
                {
                    _logger.LogInformation($"Redirection! Status Code: {statusCode}");
                }
                else
                {
                    _logger.LogInformation($"Success! Status Code: {statusCode}");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"An unexpected error occurred in the middleware: {ex.Message}");

            }
        }
    }

    public static class SampleCustomMiddlewreExtensions
    {
        public static IApplicationBuilder UseRequestHandlerMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<RequestHandlerMiddleware>();
            return builder;
        }
    }
}

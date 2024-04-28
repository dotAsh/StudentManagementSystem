

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
            _logger.LogInformation("SampleCustomMiddlewre is called***");
            _logger.LogInformation("Request Logging : " + context.Request.Method + " " + context.Request.Path + context.Request.QueryString);

            try
            {

                await _next(context);

                if (context.Response.StatusCode >= 400)
                {
                    _logger.LogError($"**********An error occured during the request processing! Status Code: {context.Response.StatusCode}");

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

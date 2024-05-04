using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

//Developed by Md. Ashik
namespace StudentManagement.API.Filters{
    public class CentralizedExceptionFilter : IExceptionFilter{
        private readonly ILogger _logger;

        public CentralizedExceptionFilter(ILogger<CentralizedExceptionFilter> logger){
            _logger = logger;
        }

        public void OnException(ExceptionContext context){
            _logger.LogError(context.Exception, "An error occurred while processing the request.");
            context.Result = new Microsoft.AspNetCore.Mvc.StatusCodeResult(500);
        }
    }

}

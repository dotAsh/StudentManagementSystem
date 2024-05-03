using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace StudentManagement.API.Filters{
    public class CustomAuthorizationFilter : IAuthorizationFilter{
        private readonly ILogger _logger;
        public CustomAuthorizationFilter(ILogger<CustomAuthorizationFilter> logger){
            _logger = logger;
        }

        public void OnAuthorization(AuthorizationFilterContext context){
           
            if (!context.HttpContext.User.Identity.IsAuthenticated){
                _logger.LogInformation("*****Unauthorized access attempted.******");
                context.Result = new Microsoft.AspNetCore.Mvc.UnauthorizedResult();
            }
        }
    }

}

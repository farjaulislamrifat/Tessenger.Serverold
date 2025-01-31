using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tessenger.Server.Hubs;

namespace Tessenger.Server.Authentication
{
    public class AuthServiceFillterForTemp : IAuthorizationFilter
    {
        private readonly IConfiguration _configuration;

        public AuthServiceFillterForTemp(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var authKey = _configuration.GetValue<string>("Authkey");

            var keys = Hub_Methods.User.Values;
            var headerTemp_Key = context.HttpContext.Request.Headers["api-Temp-key"].FirstOrDefault();
            var header_Key = context.HttpContext.Request.Headers["api-key"].FirstOrDefault();

            if (headerTemp_Key == null && header_Key == null)
            {
                context.Result = new UnauthorizedObjectResult("Invalid Authentication");

            }
            else if (headerTemp_Key != null)
            {
                if ((headerTemp_Key == null ? "" : headerTemp_Key) != authKey)
                {
                    context.Result = new UnauthorizedObjectResult("Invalid Authentication");

                }
            }
            else
            {
                if (!keys.Any(c => c.Contains(header_Key == null ? "" : header_Key)) && header_Key != "Rifat")
                {
                    context.Result = new UnauthorizedObjectResult("Invalid Authentication");
                }
            }
        }
    }
}

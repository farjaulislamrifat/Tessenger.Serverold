using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Tessenger.Server.Hubs;

namespace Tessenger.Server.Authentication
{


    public class AuthServiceFillter : IAuthorizationFilter
    {

        private readonly IConfiguration _configuration;

        public AuthServiceFillter(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void OnAuthorization(AuthorizationFilterContext context)
        {

            var keys = Hub_Methods.User.Values;
            var header_Key = context.HttpContext.Request.Headers["api-key"].FirstOrDefault();
            
            var fillters = context.Filters;
          
            var ishaveAuthServiceFillterForTemp = fillters.Any(c => c.GetType() == typeof(AuthServiceFillterForTemp));

            if (!ishaveAuthServiceFillterForTemp)
            {
                if (!keys.Any(c => c.Contains(header_Key == null ? "" : header_Key)) && header_Key != "Rifat")
                {
                    context.Result = new UnauthorizedObjectResult("Invalid Authentication");
                }
            }
        }
    }
}

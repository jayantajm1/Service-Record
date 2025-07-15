

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Service_Record.Models.Claims;
using System.Security.Claims;
using System.Text.Json;

namespace Service_Record.Filters
{

    public class AppAuthFilterAttribute : IAuthorizationFilter
    {
        private readonly string[] _roles;

        public AppAuthFilterAttribute(string roles)
        {
            _roles = roles.Split(',');
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {

            if (!context.HttpContext.Items.TryGetValue("userclaimmodel", out object? userClaims))
            {
                context.Result = new JsonResult(new { message = "UnAuthenticated", apiResponseStatus = 3, result = false }) { StatusCode = StatusCodes.Status200OK };
             
                return;
            }

            if (userClaims != null)
            {
                AuthClaimModel userclaimmodel = (AuthClaimModel)userClaims;
                List<Claim> userclaim = userclaimmodel.claims;
                bool acs = true;
                var application = userclaim.Where(c => c.Type == "application").Select(application => application).ToArray();

                var expClaim = userclaim.FirstOrDefault(c => c.Type == "exp");
                if (expClaim == null || !long.TryParse(expClaim.Value, out long expSeconds))
                {
                    context.Result = new JsonResult(new { message = "Invalid JWT expiration claim", apiResponseStatus = 3, result = false })
                    { StatusCode = StatusCodes.Status200OK };
                    return;
                }

                // Convert expiration from seconds since Unix epoch to DateTime
                var expDate = DateTimeOffset.FromUnixTimeSeconds(expSeconds).UtcDateTime;
                if (expDate <= DateTime.UtcNow)
                {
                    context.Result = new JsonResult(new { message = "JWT Expired", apiResponseStatus = 3, result = false })
                    { StatusCode = StatusCodes.Status401Unauthorized };
                    return;
                }
               
            }

        }
    }

    public class AuthorizeAttribute : TypeFilterAttribute
    {
        public AuthorizeAttribute(string roles) : base(typeof(AppAuthFilterAttribute))
        {
            Arguments = [roles];
        }
    }

}
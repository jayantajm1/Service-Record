using Microsoft.IdentityModel.Tokens;
using Service_Record.BAL.Authentication;
using Service_Record.DAL.Enums;
using Service_Record.Helper;
using Service_Record.Models.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

namespace Service_Record.Middlewares
{
    public class AuthTokenMiddleware(
        RequestDelegate next,
        ILogger<AuthTokenMiddleware> logger,
        IConfiguration configuration,
        ITokenHelper tokenHelper)
    {
        private readonly RequestDelegate _next = next;
        private readonly ILogger<AuthTokenMiddleware> _logger = logger;
        private readonly IConfiguration _config = configuration;
        private readonly ITokenHelper _tokenHelper = tokenHelper;

        public async Task Invoke(HttpContext context)
        {
            APIResponseClass<bool> response = new();
            string token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last() ?? "";

            try
            {
                //  Config values
                var secretKey = _config["Auth:SecretKey"];
                var issuer = _config["Jwt:Issuer"];
                var audience = _config["Jwt:Audience"];

                if (string.IsNullOrEmpty(secretKey) || string.IsNullOrEmpty(issuer) || string.IsNullOrEmpty(audience))
                {
                    response.apiResponseStatus = APIResponseStatus.Error;
                    response.message = "Missing Auth config (SecretKey, Issuer, Audience)";
                    response.result = false;
                    await WriteUnauthorizedResponse(context, response);
                    return;
                }

                // No token? Allow for [AllowAnonymous] endpoints
                if (string.IsNullOrWhiteSpace(token))
                {
                    await _next(context);
                    return;
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                if (!tokenHandler.CanReadToken(token))
                {
                    response.apiResponseStatus = APIResponseStatus.Error;
                    response.message = "Malformed or unreadable token.";
                    response.result = false;
                    await WriteUnauthorizedResponse(context, response);
                    return;
                }

               
                var validationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                    ValidateIssuer = true,
                    ValidIssuer = issuer,
                    ValidateAudience = true,
                    ValidAudience = audience,
                    ValidateLifetime = true,

                    ClockSkew = TimeSpan.Zero

                };

                var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

                if (principal.Identity?.IsAuthenticated != true)
                {
                    response.apiResponseStatus = APIResponseStatus.Error;
                    response.message = "Token is not authenticated.";
                    response.result = false;
                    await WriteUnauthorizedResponse(context, response);
                    return;
                }

                //  Save user claims for controllers
                context.Items["userclaimmodel"] = new AuthClaimModel
                {
                    claims = principal.Claims.ToList()
                };
                context.Items["Token"] = token;

                // Optional debug info
                var userId = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId)?.Value ?? "--";
                var role = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value ?? "--";

                _logger.LogInformation($"✅ Token verified for UserId: {userId}, Role: {role}");

                // ⏳ Remaining Time (optional)
                var exp = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
                if (long.TryParse(exp, out long expUnix))
                {
                    long remainingSeconds = expUnix - DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    context.Response.Headers.Append("Remaining-Time", remainingSeconds.ToString());
                    context.Response.Headers.Append("Access-Control-Expose-Headers", "Remaining-Time");
                }

                await _next(context); // Pass to next middleware
            }
            catch (SecurityTokenExpiredException)
            {
                response.apiResponseStatus = APIResponseStatus.Error;
                response.message = "Token expired.";
                response.result = false;
                await WriteUnauthorizedResponse(context, response);
            }
            catch (SecurityTokenNotYetValidException)
            {
                response.apiResponseStatus = APIResponseStatus.Error;
                response.message = "Token not yet active.";
                response.result = false;
                await WriteUnauthorizedResponse(context, response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error in AuthTokenMiddleware");
                response.apiResponseStatus = APIResponseStatus.Error;
                response.message = "Unexpected authentication error.";

                response.message += $" DEBUG: {ex.Message}";

                response.result = false;
                await WriteUnauthorizedResponse(context, response);
            }
        }

        private static async Task WriteUnauthorizedResponse(HttpContext context, APIResponseClass<bool> response)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            context.Response.ContentType = "application/json; charset=utf-8";
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }

    public static class AuthTokenMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthTokenMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthTokenMiddleware>();
        }
    }
}

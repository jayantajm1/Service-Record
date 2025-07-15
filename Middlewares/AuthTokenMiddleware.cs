using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Service_Record.Middlewares
{

    public class AuthTokenMiddleware(
        RequestDelegate next,
        ILogger<AuthTokenMiddleware> logger,
        IConfiguration Configuration,
        ITokenHelper tokenHelper
    )
    {
        private readonly IConfiguration _Configuration = Configuration;
        private readonly ILogger<AuthTokenMiddleware> _logger = logger;
        private readonly RequestDelegate _next = next;
        private readonly ITokenHelper _tokenHelper = tokenHelper;

        public async Task Invoke(HttpContext context)
        {
            APIResponseClass<bool> response = new();
            string tokenId = "--", token = "";
            try
            {
                token = context.Request.Headers.Authorization.FirstOrDefault()?.Split(" ").Last() ?? "";

                string aesKey = _Configuration?.GetSection("Auth:AesKey")?.Value ?? "";
                string aesIV = _Configuration?.GetSection("Auth:AesIV")?.Value ?? "";
                string secretKey = _Configuration?["Auth:SecretKey"] ?? "";
                string jwtIssuer = _Configuration?["Jwt:Issuer"] ?? "";
                string jwtAudience = _Configuration?["Jwt:Audience"] ?? "";

                if (aesIV == "" || aesKey == "" || secretKey == "" || jwtIssuer == "" || jwtAudience == "")
                {
                    response.apiResponseStatus = Enum.APIResponseStatus.Error;
                    response.message = "Missing " + aesIV == "" ? "Auth:AesIV " : ""
                        + aesKey == "" ? "Auth:AesKey " : ""
                        + secretKey == "" ? "Auth:SecretKey " : ""
                        + jwtIssuer == "" ? "Jwt:Issuer " : ""
                        + jwtAudience == "" ? "Jwt:Audience " : "".Replace(" ", ", ").Trim([',', ' '])
                        + " in configuration, check appsettings. " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") + ".json";
                    response.result = false;
                    return;
                }

                if (token != "")
                {
                    // token = AESEncrytDecry.DecryptStringAES(token, aesKey, aesIV);

                    JwtSecurityTokenHandler? tokenHandler = new();
                    if (!tokenHandler.CanReadToken(token))
                    {
                        response.message = "Malformed Token is being used check Authorization header of this request.";
#if DEBUG
                        response.message = "TokenId: " + tokenId + " is not readable.";
#endif
                        Console.WriteLine(response.message);
                        Console.WriteLine("URL: " + context.Request.GetDisplayUrl());
                        Console.WriteLine($"Malformed Token: {token}");
                        response.apiResponseStatus = Enum.APIResponseStatus.Error;
                        response.result = false;
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                        context.Response.ContentType = "application/json; charset=utf-8";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                        return;
                    }

                    JwtSecurityToken requestToken = tokenHandler.ReadJwtToken(token);
                    tokenId = requestToken.Id;

                    if (tokenId != "0")
                    {
                        ILoginLogRepository _loginLogRepository = context.RequestServices.GetRequiredService<ILoginLogRepository>();

                        LoginLog? loginLog = await _loginLogRepository.GetSingleAysnc(e => e.Id == long.Parse(tokenId));

                        string? requestTokenType = requestToken.Claims.Where(
                            claims => claims.Type == "typ"
                        ).Select(
                            claim => claim.Value
                        ).FirstOrDefault();

                        if (loginLog == null)
                        {
                            Console.WriteLine($"Use of revoked Token: {tokenId} blocked.");
                            response.message = "Unauthorized use of revoked tokenId: " + tokenId;
                            response.apiResponseStatus = Enum.APIResponseStatus.Error;
                            response.result = false;
                            context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                            context.Response.ContentType = "application/json; charset=utf-8";
                            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                            return;
                        }
                        else if (loginLog.ApplicationId != 0 && requestTokenType == "acc")
                        {
                            IApplicationRepository _applicationRepository = context.RequestServices.GetRequiredService<IApplicationRepository>();
                            secretKey = await _applicationRepository.GetSingleSelectedColumnByConditionAsync(
                                e => e.Id == loginLog.ApplicationId,
                                e => e.Key
                            );
                            Console.WriteLine($"Using App: {loginLog.ApplicationId} Token: {tokenId} SecretKey: {secretKey[..8]}...");
                        }
                    }

                    TokenValidationParameters? validationParameters = new()
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                        ValidateIssuer = true,
                        ValidIssuer = jwtIssuer,
                        ValidateAudience = true,
                        ValidAudience = jwtAudience,
                        ValidateLifetime = true, // Ensures the token has not expired
#if DEBUG
                        ClockSkew = TimeSpan.Zero
#endif
                    };

                    System.Security.Claims.ClaimsPrincipal? principal = tokenHandler.ValidateToken(
                        token,
                        validationParameters,
                        out SecurityToken validatedToken
                    );

                    bool isAuthenticated = false;

                    if (principal.Identity != null)
                    {
                        isAuthenticated = principal.Identity.IsAuthenticated;
                    }

                    AuthClaimModel? jwtAuthClaimModel = new()
                    {
                        claims = [.. principal.Claims]
                    };

                    if (isAuthenticated && jwtAuthClaimModel != null)
                    {
                        string? expiration = principal.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value ?? "0";

                        if (expiration != "0")
                        {
                            long remainingTime = long.Parse(expiration) - DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                            context.Response.Headers.Append(
                                "Remaining-Time",
                                remainingTime.ToString()
                            );
                            context.Response.Headers.Append(
                                "Access-Control-Expose-Headers",
                                "Remaining-Time"
                            );
                        }
                        context.Items["userclaimmodel"] = jwtAuthClaimModel;
                        context.Items["Token"] = token;
                        if (tokenHandler.CanReadToken(token))
                        {
                            var validatedClaims = tokenHandler.ReadJwtToken(token).Claims;
                            var userId = validatedClaims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.NameId)?.Value ?? "--";
                            var appId = validatedClaims.FirstOrDefault(c => c.Type == "aid")?.Value ?? "--";
                            Console.WriteLine($"Authorized TokenID: {validatedToken.Id} User: {userId} AppID: {appId}");
                        }
                        else
                        {
                            Console.WriteLine($"Unable to read Authorized TokenID: {validatedToken.Id}");
                        }
                        await _next(context);
                    }
                    else
                    {
                        response.message = "Invalid Token";
                        response.apiResponseStatus = Enum.APIResponseStatus.Error;
                        response.result = false;
                        context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                        context.Response.ContentType = "application/json; charset=utf-8";
                        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                        return;
                    }

                }
                else
                {
                    //Not possible to authenticate without a toekn
                    await _next(context);
                }
            }
            catch (SecurityTokenNotYetValidException ex)
            {
                response.message = "Token is not activated yet.";
#if DEBUG
                response.message = "TokenId: " + tokenId + " is not activated yet: " + ex.InnerException?.ToString() + ex.ToString();
#endif
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (SecurityTokenExpiredException ex)
            {
                response.message = "Token Expired.";
#if DEBUG
                response.message = "TokenId: " + tokenId + " expired: " + ex.InnerException?.ToString() + ex.ToString();
#endif
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                Console.WriteLine("Token(Causing Exception): " + token);
                Console.WriteLine("URL: " + context.Request.GetDisplayUrl());
                Console.WriteLine("Exception: " + ex.ToString());
                response.message = "Server Error: check server log for details.";
#if DEBUG
                response.message = "Server Error TokenId: " + tokenId
                    + ex.InnerException?.ToString() + ex.ToString();
#endif
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.result = false;
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;

                context.Response.ContentType = "application/json; charset=utf-8";
                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                return;
            }
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
using Microsoft.IdentityModel.Tokens;
using Service_Record.Models.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;


namespace Service_Record.BAL.Authentication
{
    public class TokenHelper : ITokenHelper
    {
        private readonly IConfiguration _Configuration;
        private readonly ILogger<TokenHelper> _logger;
        private readonly ITokencache _tokencache;
        private readonly string ActiveLifeTimeWindowinMint;
        private readonly string authSecretKey;

        public TokenHelper(ILogger<TokenHelper> logger, IConfiguration Configuration, ITokencache tokencache)
        {
            _logger = logger;
            _Configuration = Configuration;
            authSecretKey = _Configuration.GetValue<string>("Auth:SecretKey");
            ActiveLifeTimeWindowinMint = _Configuration.GetValue<string>("Auth:ActiveLifeTimeWindowinMint");
            _tokencache = tokencache;
        }

       
        
        /// <summary>
        /// out int LifetimeExpirtedFlag = 0 when the token is valid
        /// out int LifetimeExpirtedFlag = 1 when the token is invalid due any other reason
        /// out int LifetimeExpirtedFlag = 2 when the token is invalid due to its life time expiried only
        /// </summary>
        /// <param name="token"></param>
        /// <param name="LifetimeExpirtedFlag"></param>
        /// <returns></returns>
        public SecurityToken ValidateToken(string token, out int LifetimeExpirtedFlag)
        {
            SecurityToken validatedToken = null;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetValidationParameters();
                tokenHandler.ValidateToken(token, validationParameters, out validatedToken);
                LifetimeExpirtedFlag = 0;
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("IDX10223: Lifetime validation failed."))
                    LifetimeExpirtedFlag = 2;
                else
                    LifetimeExpirtedFlag = 1;
                //_logger.LogError(ex);
                //IDX10223: Lifetime validation failed.
                validatedToken = null;
            }

            return validatedToken;
        }

      


        public void InvalidateUserLogin(List<Guid> UserIds)
        {
            try
            {
                foreach (var userid in UserIds)
                    _tokencache.InvalidateUserToken(userid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw ex;
            }
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = false, // Because there is no expiration in the generated token
                ValidateAudience = false, // Because there is no audiance in the generated token // need to add later
                ValidateIssuer = false,   // Because there is no issuer in the generated token
                //ValidIssuer = "Sample", //// need to add later
                //ValidAudience = "Sample", // // need to add later
                ValidateIssuerSigningKey = true,
                RequireExpirationTime = true,
                // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                ClockSkew = TimeSpan.Zero,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSecretKey)) 
            };
        }

        public AuthClaimModel ValidateAndGetTokenClaims(string token, out bool RefreshedAccessTokenRecieved)
        {
            throw new NotImplementedException();
        }
    }

}
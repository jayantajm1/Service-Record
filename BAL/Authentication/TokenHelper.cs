using Microsoft.IdentityModel.Tokens;
using Service_Record.Models.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace Service_Record.BAL.Authentication
{
    public class TokenHelper : ITokenHelper
    {
        private readonly IConfiguration _config;

        public TokenHelper(IConfiguration configuration)
        {
            _config = configuration;
        }

        public string GenerateToken(AuthClaimModel model)
        {
            var key = _config["Jwt:Key"];

            if (string.IsNullOrEmpty(key))
                throw new Exception("JWT Secret Key is missing in configuration. Please check appsettings.json → Jwt:Key");

            var keyBytes = Encoding.UTF8.GetBytes(key);
            var expiryHours = _config.GetValue<int>("Jwt:TokenExpiryInHours");
            var expires = DateTime.UtcNow.AddHours(expiryHours);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Audience"],
                claims: model.claims,
                expires: expires,
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"])),
                    SecurityAlgorithms.HmacSha256
                )
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
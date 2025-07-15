using Microsoft.IdentityModel.Tokens;
using Service_Record.Models.Claims;

namespace Service_Record.BAL.Authentication
{
    public interface ITokenHelper
    {
        public string GenerateToken(AuthClaimModel userClaims);
      
    }

}

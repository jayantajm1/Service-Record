using Microsoft.IdentityModel.Tokens;
using Service_Record.Models.Claims;

namespace Service_Record.BAL.Authentication
{
    public interface ITokenHelper
    {
    
        public SecurityToken ValidateToken(string token, out int LifetimeExpirtedFlag);
        AuthClaimModel ValidateAndGetTokenClaims(string token, out bool RefreshedAccessTokenRecieved);
        public void InvalidateUserLogin(List<Guid> UserIds);
    }

}

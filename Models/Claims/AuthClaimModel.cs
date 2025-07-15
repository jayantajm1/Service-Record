using System.Security.Claims;

namespace Service_Record.Models.Claims
{
    public class AuthClaimModel
    {
        public  List<Claim> claims { get; set; }
        public  string? RefreshedAccessToken { get; set; }
    }

}

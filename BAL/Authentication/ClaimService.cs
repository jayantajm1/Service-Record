using Service_Record.Models.Claims;

namespace Service_Record.BAL.Authentication
{
    public class ClaimService : IClaimService
    {
        private readonly IHttpContextAccessor _contextAccessor;
        //private List<ClaimModel.Application> _applications = new List<ClaimModel.Application>();
        private AuthClaimModel logedinUserClaims = new AuthClaimModel();

        public ClaimService(IHttpContextAccessor contextAccessor)
        {
            _contextAccessor = contextAccessor;
            logedinUserClaims = (AuthClaimModel)_contextAccessor.HttpContext.Items["userclaimmodel"];

        }

    }
}

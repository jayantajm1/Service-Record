using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service_Record.Helper;
using Service_Record.Models.DTOs;

namespace Service_Record.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController()
        {
            
        }




        [HttpPost("UserRegistration")]
        public async Task<APIResponseClass<bool>> UserRegistration(UserRegistrationDTO user, bool isSuperAdminCreation = false)
        {
            APIResponseClass<bool> response = new();
            try
            {
                
              
                return response;
            }
            catch (Exception Ex)
            {
                response.apiResponseStatus = Enum.APIResponseStatus.Error;
                response.message = "Failed to create user. Please try again." + Ex.Message;
                return response;
            }
        }

    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service_Record.DAL.Interfaces;

namespace Service_Record.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public AuthController(IUserRepo userRepo, IJwtService jwtService)
        {
            _userRepo = userRepo;
            _jwtService = jwtService;
        }
    }
}

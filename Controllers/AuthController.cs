using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service_Record.BAL.Authentication;
using Service_Record.BAL.Interfaces;
using Service_Record.DAL.Enums;
using Service_Record.DAL.Interfaces;
using Service_Record.Helper;
using Service_Record.Models.DTOs;

namespace Service_Record.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserlogService _userlogService;

        public AuthController(IUserService userService, IUserlogService userlogService)
        {
            _userService = userService;
            _userlogService = userlogService;
        }

        /// <summary>
        /// Logs in a user.
        /// </summary>
        /// <param name="LoginDTO">User login credentials</param>
        /// <returns>API response with token</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO loginDto)
        {
            if (loginDto == null)
            {
                return BadRequest(new APIResponseClass<string>
                {
                    apiResponseStatus = APIResponseStatus.Error,
                    message = "Login credentials cannot be null."
                });
            }

            var result = await _userlogService.LoginAsync(loginDto);

            if (result.apiResponseStatus == APIResponseStatus.Success)
                return Ok(result);
            else if (result.apiResponseStatus == APIResponseStatus.Error)
                return Unauthorized(result);

            return BadRequest(result);
        }

    }
}

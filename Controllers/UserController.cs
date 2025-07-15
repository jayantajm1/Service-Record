using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service_Record.BAL.Interfaces;
using Service_Record.BAL.Services;
using Service_Record.DAL.Enums;
using Service_Record.Helper;
using Service_Record.Models.DTOs;

namespace Service_Record.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }



        /// <summary>
        /// Registers a new user.
        /// </summary>
        /// <param name="registrationDto">User registration details</param>
        /// <returns>API response with user details</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDTO registrationDto)
        {
            if (registrationDto == null)
            {
                return BadRequest(new APIResponseClass<string>
                {
                    apiResponseStatus = APIResponseStatus.Error,
                    message = "Request body cannot be null."
                });
            }

            var result = await _userService.RegisterAsync(registrationDto);

            if (result.apiResponseStatus == APIResponseStatus.Success)
            {
                return Ok(result);
            }

            return BadRequest(result);
        }

    }
}

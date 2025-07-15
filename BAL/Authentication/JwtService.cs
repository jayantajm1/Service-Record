//using Service_Record.DAL.Entities;
//using Service_Record.DTOs;
//using System.Security.Claims;

//namespace Service_Record.BAL.Authentication
//{
//    public class JwtService : IJwtService
//    {
//        private readonly IConfiguration _configuration;

//        public JwtService(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        public string GenerateToken(User user)
//        {
//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString());
//                new Claim(ClaimTypes.Name, user.Username loginDTO.Username);

//            // Check if user exists and if the password is correct
//            if (user == null || !BCrypt.Verify(loginDto.Password, user.PasswordHash))
//            {
//                return Unauthorized(new AuthResponseDTO { IsSuccess = false, Message = "Invalid username or password." });
//            }

//            // If login is successful, generate a JWT token
//            var token = _jwtService.GenerateToken(user);

//            return Ok(new AuthResponseDTO { IsSuccess = true, Message = "Login successful.", Token = token });
//        }
//    }
//}

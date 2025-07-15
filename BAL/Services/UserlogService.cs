using AutoMapper;
using Microsoft.IdentityModel.JsonWebTokens;
using Service_Record.BAL.Authentication;
using Service_Record.BAL.Interfaces;
using Service_Record.DAL.Entities;
using Service_Record.DAL.Enums;
using Service_Record.DAL.Interfaces;
using Service_Record.Helper;
using Service_Record.Models;
using Service_Record.Models.Claims;
using Service_Record.Models.DTOs;
using System.Security.Claims;



namespace Service_Record.BAL.Services
{
    public class UserlogService : IUserlogService
    {
        private readonly IUserlogRepo _userlogRepo;
        private readonly IMapper _mapper;
        private readonly IUserRepo _userRepo;
        private readonly ITokenHelper _tokenHelper;

        public UserlogService(IUserlogRepo userlogRepo, IMapper mapper, IUserRepo userRepo, ITokenHelper tokenHelper) {
            _userlogRepo = userlogRepo;
            _mapper = mapper;
            _userRepo = userRepo;
            _tokenHelper = tokenHelper;
        }

        public async Task<IEnumerable<UserlogModel>> GetAllUserlog()
        {
            var allData = await _userlogRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<UserlogModel>>(allData);
        }

        public async Task<UserLog?> GetUserLogByIdAsync(long id)
        {
            return await _userlogRepo.GetSingleAsync(log => log.LogId == id);
        }



        public async Task<APIResponseClass<AuthResponseDTO>> LoginAsync(LoginDTO loginDto)
        {
            var response = new APIResponseClass<AuthResponseDTO>();

            // Validate input
            if (loginDto == null || string.IsNullOrWhiteSpace(loginDto.Username) || string.IsNullOrWhiteSpace(loginDto.Password))
            {
                response.apiResponseStatus = APIResponseStatus.Error;
                response.message = "Username and password are required.";
                return response;
            }

            // Get user by username
            var user = await _userRepo.GetUserByUsernameAsync(loginDto.Username);

            // Check if user exists and verify hashed password
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                response.apiResponseStatus = APIResponseStatus.Error;
                response.message = "Invalid username or password.";
                return response;
            }

            // Check if user already logged in
            var existingLog = await _userlogRepo.GetSingleAsync(log => log.UserId == user.UserId);
            long userLogId;

            if (existingLog != null)
            {
                userLogId = existingLog.UserId;
            }
            else
            {
                // Create a new log
                var newLog = new UserLog
                {
                    UserId = user.UserId,
                    Action = LogAction.LOGIN, 
                    LogTime = DateTime.UtcNow,
                    
                };


                var logInsertResponse = await _userlogRepo.InsertAsync(newLog);

                if (logInsertResponse.apiResponseStatus != APIResponseStatus.Success || logInsertResponse.result == null)
                {
                    return new APIResponseClass<AuthResponseDTO>
                    {
                        apiResponseStatus = APIResponseStatus.Error,
                        message = "Login failed while saving user session."
                    };
                }

                userLogId = logInsertResponse.result.UserId;
            }

            // Prepare JWT claims
            var claims = new List<Claim>
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Username),
        new Claim(JwtRegisteredClaimNames.Jti, userLogId.ToString()),
        new Claim(JwtRegisteredClaimNames.NameId, user.UserId.ToString()),
        new Claim(ClaimTypes.Role, user.Role.ToString()), // Enum to string
        new Claim("typ", "acc") // Access token type
    };

            // Generate JWT token
            var token = _tokenHelper.GenerateToken(new AuthClaimModel { claims = claims });

            // Prepare response
            response.apiResponseStatus = APIResponseStatus.Success;
            response.message = "Login successful.";
            response.result = new AuthResponseDTO
            {
                Token = token,
               
              
            };

            return response;
        }


      



    }
}
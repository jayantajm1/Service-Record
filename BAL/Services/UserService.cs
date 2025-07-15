using AutoMapper;
using Service_Record.BAL.Interfaces;
using Service_Record.DAL.Entities;
using Service_Record.DAL.Enums;
using Service_Record.DAL.Interfaces;
using Service_Record.Helper;
using Service_Record.Models;
using Service_Record.Models.DTOs;

namespace Service_Record.BAL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;
        

        public UserService(IUserRepo userRepo, IMapper mapper) {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserModel>> GetAllUser()
        {
            var allData = await _userRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<UserModel>>(allData);
        }
        public async Task<APIResponseClass<User>> RegisterAsync(UserRegistrationDTO registrationDto)
        {
            if (string.IsNullOrWhiteSpace(registrationDto.Username))
            {
                return new APIResponseClass<User>
                {
                    apiResponseStatus = APIResponseStatus.Error,
                    message = "Username is required."
                };
            }

            if (string.IsNullOrWhiteSpace(registrationDto.Password))
            {
                return new APIResponseClass<User>
                {
                    apiResponseStatus = APIResponseStatus.Error,
                    message = "Password is required."
                };
            }

            try
            {
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(registrationDto.Password);

                var userEntity = new User
                {
                    FullName = registrationDto.FullName,
                    Username = registrationDto.Username.ToLower(),
                    Email = registrationDto.Email?.ToLower(),
                    PasswordHash = passwordHash,
                    //Role = registrationDto.Role, // ✅ Use DTO.Role if present, or assign a default
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var repositoryResponse = await _userRepo.RegisterUserAsync(userEntity);

                return repositoryResponse;
            }
            catch (Exception ex)
            {
               
               
                return new APIResponseClass<User>
                {
                    apiResponseStatus = APIResponseStatus.Error,
                    message = $"An internal service error occurred: {ex.Message}",
                    result = null
                };
            }
        }

       

    }
}
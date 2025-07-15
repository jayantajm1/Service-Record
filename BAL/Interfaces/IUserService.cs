using Service_Record.DAL.Entities;
using Service_Record.Helper;
using Service_Record.Models;
using Service_Record.Models.DTOs;

namespace Service_Record.BAL.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetAllUser();
        public  Task<APIResponseClass<User>> RegisterAsync(UserRegistrationDTO registrationDto);
    }
}
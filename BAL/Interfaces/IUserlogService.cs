using Service_Record.DAL.Entities;
using Service_Record.Helper;
using Service_Record.Models;
using Service_Record.Models.DTOs;

namespace Service_Record.BAL.Interfaces
{
    public interface IUserlogService
    {
        Task<IEnumerable<UserlogModel>> GetAllUserlog();
        public  Task<UserLog?> GetUserLogByIdAsync(long id);
        public  Task<APIResponseClass<AuthResponseDTO>> LoginAsync(LoginDTO loginDto);
    }
}
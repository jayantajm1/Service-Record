using Service_Record.DAL.Entities;
using Service_Record.Helper;
using Service_Record.Models.DTOs;
namespace Service_Record.DAL.Interfaces
{
    public interface IUserRepo: IRepository<User>
    {
        public  Task<APIResponseClass<User>> RegisterUserAsync(User user);
        public  Task<User> GetUserByUsernameAsync(string username);
    }
}
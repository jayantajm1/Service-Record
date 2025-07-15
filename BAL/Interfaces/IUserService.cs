using Service_Record.Models;

namespace Service_Record.BAL.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserModel>> GetAllUser();
    }
}
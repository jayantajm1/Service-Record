using Service_Record.Models;

namespace Service_Record.BAL.Interfaces
{
    public interface IUserlogService
    {
        Task<IEnumerable<UserlogModel>> GetAllUserlog();
    }
}
using Service_Record.Models;

namespace Service_Record.BAL.Interfaces
{
    public interface IServicerecordpartService
    {
        Task<IEnumerable<ServicerecordpartModel>> GetAllServicerecordpart();
    }
}
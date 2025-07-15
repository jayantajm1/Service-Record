using Service_Record.Models;

namespace Service_Record.BAL.Interfaces
{
    public interface IServicerecordService
    {
        Task<IEnumerable<ServicerecordModel>> GetAllServicerecord();
    }
}
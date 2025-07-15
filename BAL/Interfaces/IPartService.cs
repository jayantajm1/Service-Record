using Service_Record.Models;

namespace Service_Record.BAL.Interfaces
{
    public interface IPartService
    {
        Task<IEnumerable<PartModel>> GetAllPart();
    }
}
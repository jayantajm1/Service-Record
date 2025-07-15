using Service_Record.Models;

namespace Service_Record.BAL.Interfaces
{
    public interface IBranchService
    {
        Task<IEnumerable<BranchModel>> GetAllBranch();
    }
}
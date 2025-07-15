using AutoMapper;
using Service_Record.BAL.Interfaces;
using Service_Record.DAL.Interfaces;
using Service_Record.Models;
using Service_Record.DAL.Entities;

namespace Service_Record.BAL.Services
{
    public class BranchService : IBranchService
    {
        private readonly IBranchRepo _branchRepo;
        private readonly IMapper _mapper;

        public BranchService(IBranchRepo branchRepo, IMapper mapper) {
            _branchRepo = branchRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BranchModel>> GetAllBranch()
        {
            var allData = await _branchRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<BranchModel>>(allData);
        }
    }
}
using AutoMapper;
using Service_Record.BAL.Interfaces;
using Service_Record.DAL.Interfaces;
using Service_Record.Models;
using Service_Record.DAL.Entities;

namespace Service_Record.BAL.Services
{
    public class UserlogService : IUserlogService
    {
        private readonly IUserlogRepo _userlogRepo;
        private readonly IMapper _mapper;

        public UserlogService(IUserlogRepo userlogRepo, IMapper mapper) {
            _userlogRepo = userlogRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserlogModel>> GetAllUserlog()
        {
            var allData = await _userlogRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<UserlogModel>>(allData);
        }
    }
}
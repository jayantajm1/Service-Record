using AutoMapper;
using Service_Record.BAL.Interfaces;
using Service_Record.DAL.Interfaces;
using Service_Record.Models;
using Service_Record.DAL.Entities;

namespace Service_Record.BAL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepo _userRepo;
        private readonly IMapper _mapper;

        public UserService(IUserRepo userRepo, IMapper mapper) {
            _userRepo = userRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserModel>> GetAllUser()
        {
            var allData = await _userRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<UserModel>>(allData);
        }
    }
}
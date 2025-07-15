using AutoMapper;
using Service_Record.BAL.Interfaces;
using Service_Record.DAL.Interfaces;
using Service_Record.Models;
using Service_Record.DAL.Entities;

namespace Service_Record.BAL.Services
{
    public class PartService : IPartService
    {
        private readonly IPartRepo _partRepo;
        private readonly IMapper _mapper;

        public PartService(IPartRepo partRepo, IMapper mapper) {
            _partRepo = partRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<PartModel>> GetAllPart()
        {
            var allData = await _partRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<PartModel>>(allData);
        }
    }
}
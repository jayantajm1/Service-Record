using AutoMapper;
using Service_Record.BAL.Interfaces;
using Service_Record.DAL.Interfaces;
using Service_Record.Models;
using Service_Record.DAL.Entities;

namespace Service_Record.BAL.Services
{
    public class ServicerecordpartService : IServicerecordpartService
    {
        private readonly IServicerecordpartRepo _servicerecordpartRepo;
        private readonly IMapper _mapper;

        public ServicerecordpartService(IServicerecordpartRepo servicerecordpartRepo, IMapper mapper) {
            _servicerecordpartRepo = servicerecordpartRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServicerecordpartModel>> GetAllServicerecordpart()
        {
            var allData = await _servicerecordpartRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<ServicerecordpartModel>>(allData);
        }
    }
}
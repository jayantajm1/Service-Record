using AutoMapper;
using Service_Record.BAL.Interfaces;
using Service_Record.DAL.Interfaces;
using Service_Record.Models;
using Service_Record.DAL.Entities;

namespace Service_Record.BAL.Services
{
    public class ServicerecordService : IServicerecordService
    {
        private readonly IServicerecordRepo _servicerecordRepo;
        private readonly IMapper _mapper;

        public ServicerecordService(IServicerecordRepo servicerecordRepo, IMapper mapper) {
            _servicerecordRepo = servicerecordRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ServicerecordModel>> GetAllServicerecord()
        {
            var allData = await _servicerecordRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<ServicerecordModel>>(allData);
        }
    }
}
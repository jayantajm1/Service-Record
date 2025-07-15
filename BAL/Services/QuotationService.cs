using AutoMapper;
using Service_Record.BAL.Interfaces;
using Service_Record.DAL.Interfaces;
using Service_Record.Models;
using Service_Record.DAL.Entities;

namespace Service_Record.BAL.Services
{
    public class QuotationService : IQuotationService
    {
        private readonly IQuotationRepo _quotationRepo;
        private readonly IMapper _mapper;

        public QuotationService(IQuotationRepo quotationRepo, IMapper mapper) {
            _quotationRepo = quotationRepo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<QuotationModel>> GetAllQuotation()
        {
            var allData = await _quotationRepo.GetAllAsync();
            return _mapper.Map<IEnumerable<QuotationModel>>(allData);
        }
    }
}
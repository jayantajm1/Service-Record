using Service_Record.DAL.Entities;
using Service_Record.DAL.Interfaces;
using Service_Record.DAL.Context;

namespace Service_Record.DAL.Repositories
{
   public class QuotationRepo : Repository<Quotation, ServiceRecordDbContext>, IQuotationRepo
   {
       public QuotationRepo(ServiceRecordDbContext context) : base(context)
       {
       }
   }
}
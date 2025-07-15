

using Service_Record.DAL.Context;
using Service_Record.DAL.Entities;
using Service_Record.DAL.Interfaces;

namespace Service_Record.DAL.Repositories
{
    public class ServicerecordRepo : Repository<ServiceRecord, ServiceRecordDbContext>, IServicerecordRepo
   {
       public ServicerecordRepo(ServiceRecordDbContext context) : base(context)
       {
       }
   }
}
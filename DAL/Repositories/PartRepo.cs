using Service_Record.DAL.Entities;
using Service_Record.DAL.Interfaces;
using Service_Record.DAL.Context;

namespace Service_Record.DAL.Repositories
{
   public class PartRepo : Repository<Part, ServiceRecordDbContext>, IPartRepo
   {
       public PartRepo(ServiceRecordDbContext context) : base(context)
       {
       }
   }
}
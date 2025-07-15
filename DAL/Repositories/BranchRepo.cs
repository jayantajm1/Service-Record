using Service_Record.DAL.Entities;
using Service_Record.DAL.Interfaces;
using Service_Record.DAL.Context;

namespace Service_Record.DAL.Repositories
{
   public class BranchRepo : Repository<Branch, ServiceRecordDbContext>, IBranchRepo
   {
       public BranchRepo(ServiceRecordDbContext context) : base(context)
       {
       }
   }
}
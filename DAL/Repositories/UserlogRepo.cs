
using Service_Record.DAL.Context;
using Service_Record.DAL.Entities;
using Service_Record.DAL.Interfaces;

namespace Service_Record.DAL.Repositories
{
   public class UserlogRepo : Repository<UserLog, ServiceRecordDbContext>, IUserlogRepo
   {
       public UserlogRepo(ServiceRecordDbContext context) : base(context)
       {
       }
   }
}
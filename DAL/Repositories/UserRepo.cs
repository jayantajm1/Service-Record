using Service_Record.DAL.Entities;
using Service_Record.DAL.Interfaces;
using Service_Record.DAL.Context;

namespace Service_Record.DAL.Repositories
{
   public class UserRepo : Repository<User, ServiceRecordDbContext>, IUserRepo
   {
       public UserRepo(ServiceRecordDbContext context) : base(context)
       {
       }
   }
}
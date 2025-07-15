
using Service_Record.DAL.Entities;
using Service_Record.Helper;
using System.Linq.Expressions;

namespace Service_Record.DAL.Interfaces
{
    public interface IUserlogRepo: IRepository<UserLog>
    {
        Task<UserLog?> GetSingleAsync(Expression<Func<UserLog, bool>> predicate);
        public  Task<APIResponseClass<UserLog>> InsertAsync(UserLog entity);
    }
}
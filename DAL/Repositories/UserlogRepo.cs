
using Microsoft.EntityFrameworkCore;
using Service_Record.DAL.Context;
using Service_Record.DAL.Entities;
using Service_Record.DAL.Enums;
using Service_Record.DAL.Interfaces;
using Service_Record.Helper;
using System.Linq.Expressions;

namespace Service_Record.DAL.Repositories
{
   public class UserlogRepo : Repository<UserLog, ServiceRecordDbContext>, IUserlogRepo
   {
        private readonly ServiceRecordDbContext _context;

        public UserlogRepo(ServiceRecordDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<UserLog?> GetSingleAsync(Expression<Func<UserLog, bool>> predicate)
        {
            return await _context.UserLogs.FirstOrDefaultAsync(predicate);
        }

        public async Task<APIResponseClass<UserLog>> InsertAsync(UserLog entity)
        {
            var response = new APIResponseClass<UserLog>();

            if (entity == null)
            {
                response.apiResponseStatus = APIResponseStatus.Error;
                response.message = "Entity cannot be null.";
                return response;
            }

            try
            {
                await _context.UserLogs.AddAsync(entity);
               var savelogin= await _context.SaveChangesAsync();
                if (savelogin < 0)
                {
                    response.apiResponseStatus = APIResponseStatus.Error;
                    response.message = "Failed to insert user log.";
                    response.result = null;
                    return response;
                }

                response.apiResponseStatus = APIResponseStatus.Success;
                response.message = "User log inserted successfully.";
                response.result = entity;
            }
            catch (Exception ex)
            {
                response.apiResponseStatus = APIResponseStatus.Error;
                response.message = $"Error inserting user log: {ex.Message}";
                response.result = null;
            }

            return response;
        }


    }
}
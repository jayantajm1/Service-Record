using Microsoft.EntityFrameworkCore;
using Service_Record.DAL.Context;
using Service_Record.DAL.Entities;
using Service_Record.DAL.Enums;
using Service_Record.DAL.Interfaces;
using Service_Record.Helper;
using Service_Record.Models.DTOs;
using System.Net;

namespace Service_Record.DAL.Repositories
{
   public class UserRepo : Repository<User, ServiceRecordDbContext>, IUserRepo
   {
        private readonly ServiceRecordDbContext _context;

        public UserRepo(ServiceRecordDbContext context) : base(context)
        {
            _context = context;
        }


        /// <summary>
        /// Adds a new user entity to the database.
        /// This method's sole responsibility is data persistence.
        /// It does not perform any business logic or validation.
        /// </summary>
        /// <param name="user">The user entity to be created.</param>
        /// <returns>An APIResponse containing the created user on success, or an error on failure.</returns>
        public async Task<APIResponseClass<User>> RegisterUserAsync(User user)
        {
            var response = new APIResponseClass<User>();

            try
            {

                await _context.Users.AddAsync(user);
                var userData = await _context.SaveChangesAsync();
                if (userData < 0)
                {
                    response.apiResponseStatus = APIResponseStatus.Error;
                    response.message = "User registration failed. User object is null.";
                    return response;
                }
                response.result = user;
                response.apiResponseStatus = APIResponseStatus.Success; // 201 Created is the correct status code.
                response.message = "User registered successfully in the database.";
            }
            catch (Exception ex)
            {


                response.message = "A database error occurred: {ex.Message}";
                response.apiResponseStatus = APIResponseStatus.Error;
                response.result = null;
            }

            return response;
        }

        public async Task<User> GetUserByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
        }


    }
}
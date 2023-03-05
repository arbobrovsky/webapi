using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using webapi.Data;
using webapi.Data.Models;
using webapi.IRepository;


namespace webapi.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly EFDBContext _context;

        public UserRepository(EFDBContext context)
        {
            _context = context;
        }

        public Task<string> Delete(User obj)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Get(Guid objId)
        {
            if (objId != Guid.Empty)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == objId);
                if (user != null)
                {
                    user.Password = string.Empty;
                    return user;
                }
            }
            return new User();
        }

        public async Task<User> GetByUsernamePassword(User user)
        {
            var userFromDb = await _context.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == user.Username && x.Password == user.Password);
            if (userFromDb != null)
                return new User
                {
                    Email = userFromDb.Email,
                    Message = userFromDb.Message,
                    Token = userFromDb.Token,
                    UserId = userFromDb.UserId,
                    Username = userFromDb.Username
                };
            return new User();
        }

        public Task<List<User>> Gets()
        {
            throw new NotImplementedException();
        }

        public async Task<User> Save(User obj)
        {
            if (obj.UserId != Guid.Empty)
            {
                var modelFromDb = await _context.Users.FirstOrDefaultAsync(x => x.UserId == obj.UserId);
                if (modelFromDb != null)
                {
                    modelFromDb.Username = obj.Username;
                    modelFromDb.LastName = obj.LastName;
                    modelFromDb.Bio = obj.Bio;
                    modelFromDb.FirstName = obj.FirstName;
                    modelFromDb.Bio = obj.Bio;
                    modelFromDb.Company = obj.Company;
                    modelFromDb.Phone = obj.Phone;
                    modelFromDb.Location = obj.Location;
                    modelFromDb.Email = obj.Email;
                    _context.Entry(modelFromDb).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return modelFromDb;
                }
            }
            else
            {
                var anyModelFromDb = await _context.Users.AnyAsync(x => x.Username.Contains(obj.Username) && x.Email.Contains(obj.Email));
                if (!anyModelFromDb)
                {
                    await _context.Users.AddAsync(obj);
                    await _context.SaveChangesAsync();
                }
                else
                {
                    var user = new User();
                    user.Message = "Пользователь с таким именем и/или почтай уже существует";
                    return user;
                }
            }
            return new User();
        }
    }
}

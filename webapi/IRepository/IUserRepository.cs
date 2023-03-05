

using webapi.Data.Models;

namespace webapi.IRepository
{
    public interface IUserRepository
    {
        Task<User> Save(User obj);
        Task<User> Get(Guid objId);
        Task<List<User>> Gets();
        Task<User> GetByUsernamePassword(User user);
        Task<string> Delete(User obj);
    }
}

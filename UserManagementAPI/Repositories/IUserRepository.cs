using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories;

public interface IUserRepository
{
    IEnumerable<User> GetAll();
    User? Get(Guid id);
    User Add(User user);
    bool Update(User user);
    bool Delete(Guid id);
}

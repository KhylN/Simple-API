using System.Collections.Concurrent;
using UserManagementAPI.Models;

namespace UserManagementAPI.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private readonly ConcurrentDictionary<Guid, User> _store = new();

    public IEnumerable<User> GetAll() => _store.Values;

    public User? Get(Guid id) => _store.TryGetValue(id, out var u) ? u : null;

    public User Add(User user)
    {
        user.Id = Guid.NewGuid();
        _store[user.Id] = user;
        return user;
    }

    public bool Update(User user)
    {
        if (!_store.ContainsKey(user.Id)) return false;
        _store[user.Id] = user;
        return true;
    }

    public bool Delete(Guid id) => _store.TryRemove(id, out _);
}

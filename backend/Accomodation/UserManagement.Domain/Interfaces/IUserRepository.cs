using UserManagement.Domain.Entities;

namespace UserManagement.Domain.Interfaces;

public interface IUserRepository
{
    public Task<User> CreateAsync(User user);
    public Task<ICollection<User>> GetAllAsync();
    public Task<User> GetAsync(Guid id);
    public Task UpdateAsync(Guid id, User user);
    public Task RemoveAsync(Guid id);
}

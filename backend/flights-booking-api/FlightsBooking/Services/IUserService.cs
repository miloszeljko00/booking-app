using FlightsBooking.Models;

namespace FlightsBooking.Services
{
    public interface IUserService
    {
        Task CreateAsync(User newUser);
        Task<User?> GetAsync(Guid id);
        Task<User?> GetByKeyAsync(string key);
        Task RemoveAsync(Guid id);
        Task UpdateAsync(Guid id, User user);
    }
}
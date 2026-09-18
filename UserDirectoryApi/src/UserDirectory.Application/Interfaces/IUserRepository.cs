#nullable enable
namespace UserDirectory.Application.Interfaces;

using UserDirectory.Domain.Entities;

public interface IUserRepository
{
    Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default);
    Task<User?> GetByIdAsync(int id, CancellationToken ct = default);
    Task AddAsync(User user, CancellationToken ct = default);
    Task UpdateAsync(User user, CancellationToken ct = default);
    Task DeleteAsync(int id, CancellationToken ct = default);
}

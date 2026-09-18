#nullable enable
using Microsoft.EntityFrameworkCore;
using UserDirectory.Application.Interfaces;
using UserDirectory.Domain.Entities;

namespace UserDirectory.Infrastructure.Persistence;

public class UserRepository : IUserRepository
{
    private readonly UserDbContext _db;

    public UserRepository(UserDbContext db) => _db = db;

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken ct = default) =>
        await _db.Users.AsNoTracking().OrderBy(u => u.Name).ToListAsync(ct);

    public async Task<User?> GetByIdAsync(int id, CancellationToken ct = default) =>
        await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task AddAsync(User user, CancellationToken ct = default)
    {
        _db.Users.Add(user);
        await _db.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(User user, CancellationToken ct = default)
    {
        _db.Users.Update(user);
        await _db.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(int id, CancellationToken ct = default)
    {
        var u = await _db.Users.FindAsync(new object[] { id }, ct);
        if (u is null) return;
        _db.Users.Remove(u);
        await _db.SaveChangesAsync(ct);
    }
}

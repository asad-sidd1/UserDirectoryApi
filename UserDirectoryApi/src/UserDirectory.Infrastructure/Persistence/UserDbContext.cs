#nullable enable
using Microsoft.EntityFrameworkCore;
using UserDirectory.Domain.Entities;

namespace UserDirectory.Infrastructure.Persistence;

public class UserDbContext : DbContext
{
    public UserDbContext(DbContextOptions<UserDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
}

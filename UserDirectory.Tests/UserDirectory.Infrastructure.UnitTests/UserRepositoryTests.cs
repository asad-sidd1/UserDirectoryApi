#nullable enable
using FluentAssertions;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;
using NUnit.Framework;
using System.Linq;
using System.Threading.Tasks;
using UserDirectory.Domain.Entities;
using UserDirectory.Infrastructure.Persistence;

namespace UserDirectory.UnitTests;

[TestFixture]
public class UserRepositoryTests
{
    private static UserDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<UserDbContext>()
            .UseInMemoryDatabase(System.Guid.NewGuid().ToString())
            .Options;
        return new UserDbContext(options);
    }

    [Test]
    public async Task GetAllAsync_ReturnsSeededUsers()
    {
        await using var ctx = CreateContext();
        ctx.Users.AddRange(
            new User { Name = "Alice", Age = 30, City = "X", State = "Y", Pincode = "1111" },
            new User { Name = "Bob", Age = 25, City = "A", State = "B", Pincode = "2222" });
        await ctx.SaveChangesAsync();

        var repo = new UserRepository(ctx);
        var users = (await repo.GetAllAsync()).ToList();

        users.Should().HaveCount(2);
        users.Select(u => u.Name).Should().Contain(new[] { "Alice", "Bob" });
    }

    [Test]
    public async Task AddUpdateDelete_Workflow_Works()
    {
        await using var ctx = CreateContext();
        var repo = new UserRepository(ctx);

        var user = new User { Name = "Charlie", Age = 40, City = "C", State = "D", Pincode = "3333" };
        await repo.AddAsync(user);

        var retrieved = await repo.GetByIdAsync(user.Id);
        retrieved.Should().NotBeNull();
        retrieved!.Name.Should().Be("Charlie");

        retrieved.Age = 41;
        await repo.UpdateAsync(retrieved);

        var updated = await repo.GetByIdAsync(user.Id);
        updated!.Age.Should().Be(41);

        await repo.DeleteAsync(user.Id);
        var deleted = await repo.GetByIdAsync(user.Id);
        deleted.Should().BeNull();
    }
}
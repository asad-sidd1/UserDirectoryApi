#nullable enable
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UserDirectory.Application.Interfaces;
using UserDirectory.Infrastructure.Persistence;

namespace UserDirectory.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var conn = config.GetConnectionString("DefaultConnection") ?? "Data Source=./data/userdirectory.db";
        services.AddDbContext<UserDbContext>(opt => opt.UseSqlite(conn));

        services.AddScoped<IUserRepository, UserRepository>();

        return services;
    }
}

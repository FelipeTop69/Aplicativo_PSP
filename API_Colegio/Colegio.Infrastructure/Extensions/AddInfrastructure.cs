using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Colegio.Infrastructure.Extensions;
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var cs = config.GetConnectionString("SqlServer")!;
        services.AddDbContext<Persistence.ColegioDbContext>(opt =>
            opt.UseSqlServer(cs, b => b.MigrationsAssembly(typeof(Persistence.ColegioDbContext).Assembly.FullName)));

        // Aquí registras Repositorios y UoW si los usas:
        // services.AddScoped<IStudentRepository, StudentRepository>();
        // services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}

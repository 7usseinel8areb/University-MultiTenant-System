using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using UniversitySystem.Application.Interfaces;
using UniversitySystem.Infrastructure.Data;
using UniversitySystem.Infrastructure.Factory;
using UniversitySystem.Infrastructure.Services;
using UniversitySystem.Infrastructure.TenantServices;

namespace UniversitySystem.Infrastructure.Extentions;

public static class ConfigureServices
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<MasterDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("TenantDatabase");
            options.UseSqlServer(connectionString);
        });

        services.AddScoped<ITenantService, TenantService>();
        services.AddScoped<ITenantCreatorService, TenantCreatorService>();
        services.AddScoped<IDbContextFactory<FacultyDbContext>, FacultyDbContextFactory>();
        services.AddHttpContextAccessor();
        services.AddDbContext<FacultyDbContext>((serviceProvider, options) =>
        {
            var tenantService = serviceProvider.GetRequiredService<ITenantService>();
            var tenant = tenantService.CurrentTenant;
            if (tenant == null)
                throw new InvalidOperationException("Tenant not selected");

            options.UseSqlServer(tenant.ConnectionString);
        });

        return services;
    }
}

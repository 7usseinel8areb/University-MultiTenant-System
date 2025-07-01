using Microsoft.EntityFrameworkCore;
using UniversitySystem.Infrastructure.Data;
using UniversitySystem.Infrastructure.Services;

namespace UniversitySystem.Infrastructure.Factory;

public class FacultyDbContextFactory : IDbContextFactory<FacultyDbContext>
{
    private readonly ITenantService _tenantService;

    public FacultyDbContextFactory(ITenantService tenantService)
    {
        _tenantService = tenantService;
    }

    public FacultyDbContext CreateDbContext()
    {
        var tenant = _tenantService.CurrentTenant ?? throw new InvalidOperationException("No tenant selected");
        var options = new DbContextOptionsBuilder<FacultyDbContext>()
            .UseSqlServer(tenant.ConnectionString)
            .Options;

        return new FacultyDbContext(options);
    }
}

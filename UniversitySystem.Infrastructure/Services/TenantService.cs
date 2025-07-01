using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain.Models;
using UniversitySystem.Infrastructure.Data;

namespace UniversitySystem.Infrastructure.Services;
public class TenantService : ITenantService
{
    private readonly MasterDbContext _masterDb;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IDbContextFactory<FacultyDbContext> _dbContextFactory;

    public Tenant? CurrentTenant { get; private set; }

    public TenantService(MasterDbContext masterDb, IHttpContextAccessor httpContextAccessor)
    {
        _masterDb = masterDb;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task SetTenantAsync(string tenantCode)
    {
        var tenant = await _masterDb.Tenants.FirstOrDefaultAsync(t => t.Code == tenantCode);
        if (tenant == null)
            throw new Exception("Tenant Not Found");

        CurrentTenant = tenant;

        if (_httpContextAccessor.HttpContext != null)
        {
            _httpContextAccessor.HttpContext.Items["Tenant"] = tenant;
        }
    }
}
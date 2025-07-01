using UniversitySystem.Domain.Models;

namespace UniversitySystem.Infrastructure.Services;

public interface ITenantService
{
    Tenant? CurrentTenant { get; }
    Task SetTenantAsync(string tenantCode);
}
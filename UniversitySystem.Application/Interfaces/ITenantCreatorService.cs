using UniversitySystem.Domain.Models;

namespace UniversitySystem.Application.Interfaces;

public interface ITenantCreatorService
{
    Task<Tenant> CreateTenantAsync(string name, string code);
}

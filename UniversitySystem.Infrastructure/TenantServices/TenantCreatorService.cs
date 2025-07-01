using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UniversitySystem.Application.Interfaces;
using UniversitySystem.Domain.Models;
using UniversitySystem.Infrastructure.Data;

namespace UniversitySystem.Infrastructure.TenantServices;

public class TenantCreatorService(MasterDbContext masterDb, IConfiguration configuration) : ITenantCreatorService
{
    private readonly MasterDbContext _masterDb = masterDb ?? throw new ArgumentNullException(nameof(masterDb));
    private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

    public async Task<Tenant> CreateTenantAsync(string name, string code)
    {
        var cleanCode = code.StartsWith("Faculty_", StringComparison.OrdinalIgnoreCase)
            ? code["Faculty_".Length..]
            : code;

        var dbName = $"Faculty_{cleanCode.ToUpper()}";
        var connectionString = _configuration.GetConnectionString("TenantTemplate")
            ?.Replace("{DB_NAME}", dbName)
            ?? throw new Exception("TenantTemplate connection string is missing.");

        var existingTenant = await _masterDb.Tenants.FirstOrDefaultAsync(t => t.Code == code);

        if (existingTenant != null)
        {
            var existingOptions = new DbContextOptionsBuilder<FacultyDbContext>()
                .UseSqlServer(existingTenant.ConnectionString);

            using var existingContext = new FacultyDbContext(existingOptions.Options);
            await existingContext.Database.MigrateAsync();

            return existingTenant;
        }

        var tenant = new Tenant
        {
            Name = name,
            Code = code,
            ConnectionString = connectionString
        };

        _masterDb.Tenants.Add(tenant);
        await _masterDb.SaveChangesAsync();

        var newOptions = new DbContextOptionsBuilder<FacultyDbContext>()
            .UseSqlServer(connectionString);

        using var newFacultyDb = new FacultyDbContext(newOptions.Options);
        await newFacultyDb.Database.MigrateAsync();

        return tenant;
    }

}

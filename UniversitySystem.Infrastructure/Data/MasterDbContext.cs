using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain.Models;

namespace UniversitySystem.Infrastructure.Data;

public class MasterDbContext : DbContext
{
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public MasterDbContext(DbContextOptions<MasterDbContext> options) : base(options) { }
}

using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain.Models;

namespace UniversitySystem.Infrastructure.Data;

public class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Tenant> Tenants { get; set; } = null!;
}

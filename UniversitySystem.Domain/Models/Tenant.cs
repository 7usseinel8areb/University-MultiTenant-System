namespace UniversitySystem.Domain.Models;

public class Tenant
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string Code { get; set; } = default!; // "ENG", "MED", "BUS"
    public string ConnectionString { get; set; } = default!;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

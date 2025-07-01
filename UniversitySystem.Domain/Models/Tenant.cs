namespace UniversitySystem.Domain.Models;

public class Tenant
{
    public int Id { get; set; }
    public string FacultyName { get; set; } = null!;
    public string FacultyCode { get; set; } = null!;
    public string ConnectionString { get; set; } = null!;
    public string DbProvider { get; set; } = "mssql";
    public bool IsActive { get; set; } = true;
}

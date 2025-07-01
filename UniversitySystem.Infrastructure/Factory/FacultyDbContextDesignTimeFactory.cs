using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using UniversitySystem.Infrastructure.Data;

namespace UniversitySystem.Infrastructure.Factory;

public class FacultyDbContextDesignTimeFactory : IDesignTimeDbContextFactory<FacultyDbContext>
{
    public FacultyDbContext CreateDbContext(string[] args)
    {
        // تحميل إعدادات التكوين من appsettings.json
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        // سلسلة الاتصال مع اسم قاعدة بيانات مؤقت لغرض الميجريشن فقط
        var connectionString = config.GetConnectionString("TenantTemplate")
            ?.Replace("{DB_NAME}", "Faculty_Temp");

        if (string.IsNullOrWhiteSpace(connectionString))
            throw new InvalidOperationException("TenantTemplate connection string is missing or invalid.");

        var optionsBuilder = new DbContextOptionsBuilder<FacultyDbContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new FacultyDbContext(optionsBuilder.Options);
    }
}

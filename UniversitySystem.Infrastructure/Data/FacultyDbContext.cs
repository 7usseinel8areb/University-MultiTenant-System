using Microsoft.EntityFrameworkCore;
using UniversitySystem.Domain.Models;

namespace UniversitySystem.Infrastructure.Data;

public class FacultyDbContext : DbContext
{
    public DbSet<Student> Students => Set<Student>();
    //public DbSet<Course> Courses => Set<Course>();
    //public DbSet<Instructor> Instructors => Set<Instructor>();

    public FacultyDbContext(DbContextOptions<FacultyDbContext> options) : base(options) { }
}

using Microsoft.EntityFrameworkCore;
using UniversitySystem.API.Middlewares;
using UniversitySystem.Infrastructure.Data;
using UniversitySystem.Infrastructure.Extentions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var masterDb = scope.ServiceProvider.GetRequiredService<MasterDbContext>();
    masterDb.Database.Migrate();

    var tenants = masterDb.Tenants.ToList();
    foreach (var tenant in tenants)
    {
        var optionsBuilder = new DbContextOptionsBuilder<FacultyDbContext>();
        optionsBuilder.UseSqlServer(tenant.ConnectionString);

        using var facultyDb = new FacultyDbContext(optionsBuilder.Options);
        facultyDb.Database.Migrate();
    }
}


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseMiddleware<TenantMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();

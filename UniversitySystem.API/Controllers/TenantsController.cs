using Microsoft.AspNetCore.Mvc;
using UniversitySystem.Application.DTOs;
using UniversitySystem.Application.Interfaces;

namespace UniversitySystem.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TenantsController : ControllerBase
{
    private readonly ITenantCreatorService _tenantCreatorService;

    public TenantsController(ITenantCreatorService tenantCreatorService)
    {
        _tenantCreatorService = tenantCreatorService;
    }

    /// <summary>
    /// إنشاء كلية جديدة (تينانت) وتطبيق الميجريشن الخاصة بها تلقائيًا
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> CreateTenant([FromBody] TenantDto request)
    {
        if (string.IsNullOrWhiteSpace(request.FacultyName) || string.IsNullOrWhiteSpace(request.FacultyCode))
            return BadRequest(new { Error = "Name and Code are required." });

        try
        {
            var tenant = await _tenantCreatorService.CreateTenantAsync(request.FacultyName, request.FacultyCode);

            return Ok(new
            {
                Message = "Tenant created or updated successfully.",
                Tenant = new
                {
                    tenant.Id,
                    tenant.Name,
                    tenant.Code,
                    tenant.ConnectionString
                }
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new
            {
                Error = ex.Message
            });
        }
    }
}

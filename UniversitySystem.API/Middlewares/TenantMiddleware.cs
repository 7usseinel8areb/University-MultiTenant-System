using UniversitySystem.Infrastructure.Services;

namespace UniversitySystem.API.Middlewares;

public class TenantMiddleware
{
    private readonly RequestDelegate _next;
    public TenantMiddleware(RequestDelegate next) => _next = next;

    public async Task Invoke(HttpContext context, ITenantService tenantService)
    {
        var path = context.Request.Path.Value;

        if (path != null && path.StartsWith("/api/tenants", StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue("Faculty-Code", out var code))
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync("Faculty-Code header is missing.");
            return;
        }

        try
        {
            await tenantService.SetTenantAsync(code!);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync($"Tenant not found: {ex.Message}");
            return;
        }

        await _next(context);
    }

}

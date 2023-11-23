using Application;

namespace Api.Middleware;

public class DatabaseInstallerMiddleware
{
    private readonly RequestDelegate _next;
    
    public DatabaseInstallerMiddleware(RequestDelegate next)
    {
        _next = next;
    }
 
    public async Task Invoke(HttpContext httpContext, ApplicationDbContext dbContext)
    {
        await dbContext.Database.EnsureCreatedAsync();
        await _next.Invoke(httpContext);
    }
}
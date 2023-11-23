using Api.Middleware;
using Application;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services.AddCors(options => options.AddDefaultPolicy(
    policy => policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
    ));

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddHttpContextAccessor();
builder.Services.AddHealthChecks();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(x =>
{
    x.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = $"Commerce.Api",
        Version = "v1",
    });
    
    x.TagActionsBy(api =>
    {
        if (api.GroupName != null)
            return new[] { api.GroupName };
        
        if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            return new[] { controllerActionDescriptor.ControllerName };

        throw new InvalidOperationException("Unable to determine tag for endpoint.");
    });
    
    x.DocInclusionPredicate((name, api) => true);
});

var app = builder.Build();

app.UseCors();
app.UseSwagger();
app.UseSwaggerUI(x =>
{
    x.DefaultModelsExpandDepth(-1); // Disable swagger schemas at bottom
});

app.UseMiddleware<DatabaseInstallerMiddleware>();
app.MapControllers();
app.Run();

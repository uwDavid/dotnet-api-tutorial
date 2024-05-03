using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace ApiDemo.Filters.OperationFilter;

// to add auth header for swagger
public class AuthorizationHeaderOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (operation.Security == null)
            operation.Security = new List<OpenApiSecurityRequirement>();

        var scheme = new OpenApiSecurityScheme
        {
            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
        };
        operation.Security.Add(new OpenApiSecurityRequirement
        {
            [scheme] = new List<string>()
        });
    }
}
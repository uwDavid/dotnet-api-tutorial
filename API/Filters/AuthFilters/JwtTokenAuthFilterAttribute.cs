using ApiDemo.Authority;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ApiDemo.Filters.AuthFilters;

public class JwtTokenAuthFilterAttribute : Attribute, IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        // read header for token => store token in var
        // key: Authorization, Value: toekn
        if (!context.HttpContext.Request.Headers.TryGetValue("Authorization", out var token))
        {
            context.Result = new UnauthorizedResult(); // Short-circuit on fail
            return;
        }

        var config = context.HttpContext.RequestServices.GetService<IConfiguration>();

        if (!Authenticator.VerifyToken(token, config.GetValue<string>("SecretKey")))
        {
            context.Result = new UnauthorizedResult(); // Short-circuit on fail
        }
    }
}
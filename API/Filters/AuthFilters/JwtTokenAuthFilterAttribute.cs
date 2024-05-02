using ApiDemo.Attributes;
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

        var claims = Authenticator.VerifyToken(token, config.GetValue<string>("SecretKey"));

        if (claims is null)
        {
            context.Result = new UnauthorizedResult(); // 401 - short cirtuit
        }
        else // have claims => perform authorization checks on claims
        {
            // grab required claims via Action Context of the Endpoint Method
            var requiredClaims = context.ActionDescriptor.EndpointMetadata
                .OfType<RequiredClaimAttribute>()
                .ToList();

            // 403 - checks if all claims are satisfied
            if (requiredClaims != null &&
                !requiredClaims.All(rc => claims.Any(
                    c => c.Type.ToLower() == rc.ClaimType.ToLower() &&
                        c.Value.ToLower() == rc.ClaimValue.ToLower())))
            {
                // if claims not satisfied - 403
                context.Result = new StatusCodeResult(403);
            }
        }
    }
}
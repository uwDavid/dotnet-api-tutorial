using ApiDemo.Authority;

using Microsoft.AspNetCore.Mvc;

namespace ApiDemo.Controller;

[ApiController]
public class AuthorityController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AuthorityController(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    [HttpPost("auth")] // localhost:5000/auth
    public IActionResult Authenticate([FromBody] AppCredential credential)
    {
        if (Authenticator.Authenticate(credential.ClientId, credential.Secret))
        {
            var expiresAt = DateTime.UtcNow.AddMinutes(10);
            var secretKey = _configuration.GetValue<string>("SecretKey");

            return Ok(new
            {
                access_token = Authenticator.CreateToken(credential.ClientId, expiresAt, secretKey),
                expires_at = expiresAt
            });
        }
        else
        {
            // we can access Model State directly from controller, don't need context
            ModelState.AddModelError("Unauthorized", "You are not authorized.");
            var problemDetails = new ValidationProblemDetails(ModelState)
            {
                Status = StatusCodes.Status401Unauthorized
            };
            return new UnauthorizedObjectResult(problemDetails);
        }
    }
}
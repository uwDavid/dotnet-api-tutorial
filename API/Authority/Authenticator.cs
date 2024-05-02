using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using Microsoft.IdentityModel.Tokens;

namespace ApiDemo.Authority;

public static class Authenticator
{
    public static bool Authenticate(string clientId, string secret)
    {
        var app = AppRepository.GetApplicationByClientId(clientId);
        if (app is null) return false;

        return (app.ClientId == clientId && app.Secret == secret);
    }

    public static string CreateToken(string clientId, DateTime expiresAt, string strSecretKey)
    {
        var app = AppRepository.GetApplicationByClientId(clientId);
        // JWT : Algo | Payload (claims) | Signature
        var claims = new List<Claim>
        {
            new Claim("AppName", app?.ApplicationName??string.Empty),
            // new Claim("Read", (app?.Scopes??string.Empty).Contains("read")?"true":"false"),
            // new Claim("Write", (app?.Scopes??string.Empty).Contains("write")?"true":"false"),
        };

        // read scope from AppCredential
        var scopes = app?.Scopes?.Split(",");
        if (scopes is not null && scopes.Length > 0)
        {
            foreach (var scope in scopes)
            {
                claims.Add(new Claim(scope.ToLower(), "true"));
            }
        }

        var secretKey = Encoding.ASCII.GetBytes(strSecretKey);
        var jwt = new JwtSecurityToken(
            signingCredentials: new SigningCredentials(
                new SymmetricSecurityKey(secretKey),
                SecurityAlgorithms.HmacSha256Signature
            ),
            claims: claims,
            expires: expiresAt,
            notBefore: DateTime.UtcNow
        );
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public static IEnumerable<Claim>? VerifyToken(string token, string key)
    {
        if (string.IsNullOrWhiteSpace(token)) return null;

        // to work with bearer token
        if (token.StartsWith("Bearer"))
        {
            token = token.Substring(6).Trim();
        }

        var secretKey = Encoding.ASCII.GetBytes(key);
        SecurityToken securityToken;

        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(
                token,
                new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateLifetime = true,
                    ValidateAudience = false,
                    ValidateIssuer = false,
                    ClockSkew = TimeSpan.Zero
                }, out securityToken);

            if (securityToken is not null)
            {
                var tokenObject = tokenHandler.ReadJwtToken(token);
                return tokenObject.Claims ?? (new List<Claim>());
                // verification success, but no claims
            }
            else
            {
                return null; // failed auth
            }
        }
        catch (SecurityTokenException)
        {
            return null;
        }
        catch
        {
            throw;
        }
        // if security token is not null => return true
        // return securityToken != null; - no longer required, return null above
    }
}
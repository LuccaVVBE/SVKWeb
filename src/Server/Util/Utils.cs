using System.IdentityModel.Tokens.Jwt;

namespace Svk.Server.Util;

public class Utils
{
    public static string? GetAuth0IdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var auth0IdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub");

        //split auth0IdClaim.Value on '|' and return the last part
        var id = auth0IdClaim?.Value.Split('|').Last();
        return id;
    }
}
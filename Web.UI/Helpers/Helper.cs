using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

public static class JwtHelper
{
    public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        var handler = new JwtSecurityTokenHandler();
        var token = handler.ReadJwtToken(jwt);
        return token.Claims;
    }
}
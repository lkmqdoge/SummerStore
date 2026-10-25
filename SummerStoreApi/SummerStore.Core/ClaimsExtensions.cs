using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace SummerStore.Core;

public static class ClaimsExtensions
{
    public static Guid GetUserId(this ClaimsPrincipal principal)
        => Guid.Parse(principal.FindFirstValue(JwtRegisteredClaimNames.Sub)!);
}

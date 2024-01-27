using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication.Internal;

namespace GameStore.Client.Auth0;

public class ArrayClaimsPrincipalFactory : AccountClaimsPrincipalFactory<RemoteUserAccount>
{
    public ArrayClaimsPrincipalFactory(IAccessTokenProviderAccessor accessor)
        : base(accessor)
    {
    }

    public async override ValueTask<ClaimsPrincipal> CreateUserAsync(
        RemoteUserAccount account,
        RemoteAuthenticationUserOptions options)
    {
        var user = await base.CreateUserAsync(account, options);
        var claimsIdentity = (ClaimsIdentity?)user.Identity;

        if (account != null && claimsIdentity != null)
        {
            MapArrayClaimsToMultipleSeparateClaims(account, claimsIdentity);
        }

        return user;
    }

    private static void MapArrayClaimsToMultipleSeparateClaims(RemoteUserAccount account, ClaimsIdentity claimsIdentity)
    {
        foreach (var prop in account.AdditionalProperties)
        {
            var key = prop.Key;
            var value = prop.Value;
            if (value != null && value is JsonElement element && element.ValueKind == JsonValueKind.Array)
            {
                // Remove the claim with an array value and create a separate one for each role.
                claimsIdentity.RemoveClaim(claimsIdentity.FindFirst(prop.Key));
                var claims = element.EnumerateArray().Select(x => new Claim(prop.Key, x.ToString()));
                claimsIdentity.AddClaims(claims);
            }
        }
    }
}
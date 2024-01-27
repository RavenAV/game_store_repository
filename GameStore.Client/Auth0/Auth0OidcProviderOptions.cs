using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;

namespace GameStore.Client.Auth0;

public class Auth0OidcProviderOptions : OidcProviderOptions
{
    public MetadataSeed MetadataSeed { get; set; } = new ();
}

public class MetadataSeed
{
    [JsonPropertyName("end_session_endpoint")]
    public string EndSessionEndpoint { get; set; } = null!;
}
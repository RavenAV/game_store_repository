using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.Options;

namespace GameStore.Client.Auth0;

internal class DefaultAuth0OidcOptionsConfiguration : IPostConfigureOptions<RemoteAuthenticationOptions<Auth0OidcProviderOptions>>
{
    private readonly NavigationManager _navigationManager;

    public DefaultAuth0OidcOptionsConfiguration(NavigationManager navigationManager) => _navigationManager = navigationManager;

    public void Configure(RemoteAuthenticationOptions<Auth0OidcProviderOptions> options)
    {
        options.UserOptions.AuthenticationType ??= options.ProviderOptions.ClientId;

        var redirectUri = options.ProviderOptions.RedirectUri;
        if (redirectUri == null || !Uri.TryCreate(redirectUri, UriKind.Absolute, out _))
        {
            redirectUri ??= "authentication/login-callback";
            options.ProviderOptions.RedirectUri = _navigationManager
                .ToAbsoluteUri(redirectUri).AbsoluteUri;
        }

        var logoutUri = options.ProviderOptions.PostLogoutRedirectUri;
        if (logoutUri == null || !Uri.TryCreate(logoutUri, UriKind.Absolute, out _))
        {
            logoutUri ??= "authentication/logout-callback";
            options.ProviderOptions.PostLogoutRedirectUri = _navigationManager
                .ToAbsoluteUri(logoutUri).AbsoluteUri;
        }
    }

    public void PostConfigure(string? name, RemoteAuthenticationOptions<Auth0OidcProviderOptions> options)
    {
        if (string.Equals(name, Options.DefaultName))
        {
            Configure(options);
        }
    }
}
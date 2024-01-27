using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace GameStore.Client.Auth0;

public static class AuthExtensions
{
    /// <summary>
    /// Adds support for Auth0 authentication for SPA applications using <see cref="Auth0OidcProviderOptions"/> and the <see cref="RemoteAuthenticationState"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configure">An action that will configure the <see cref="RemoteAuthenticationOptions{TProviderOptions}"/>.</param>
    /// <returns>The <see cref="IServiceCollection"/> where the services were registered.</returns>
    public static IRemoteAuthenticationBuilder<RemoteAuthenticationState, RemoteUserAccount> AddAuth0OidcAuthentication(this IServiceCollection services, Action<RemoteAuthenticationOptions<Auth0OidcProviderOptions>> configure)
    {
        services.TryAddEnumerable(ServiceDescriptor.Scoped<IPostConfigureOptions<RemoteAuthenticationOptions<Auth0OidcProviderOptions>>, DefaultAuth0OidcOptionsConfiguration>());
        return services.AddRemoteAuthentication<RemoteAuthenticationState, RemoteUserAccount, Auth0OidcProviderOptions>(configure);
    }
}
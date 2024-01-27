using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GameStore.Client;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using GameStore.Client.Auth0;
using GameStore.Client.Clients;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var gameStoreApiUrl = builder.Configuration["GameStoreApiUrl"] ?? throw new Exception("GameStoreApiUrl is not set");
var audience = builder.Configuration["Auth0:Audience"] ?? throw new Exception("Auth0:Audience is not set");
var authority = builder.Configuration["Auth0:Authority"];
var clientId = builder.Configuration["Auth0:ClientId"];

builder.Services.AddHttpClient<AuthenticatedClient>(client => client.BaseAddress = new Uri(gameStoreApiUrl))
    .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
    .ConfigureHandler(
        authorizedUrls: new[] { gameStoreApiUrl },
        scopes: new[] { "games:read", "games:write" }));

builder.Services.AddHttpClient<NotAuthenticatedClient>(client => client.BaseAddress = new Uri(gameStoreApiUrl));
builder.Services.AddHttpClient<ImagesClient>(client => client.BaseAddress = new Uri(gameStoreApiUrl))
    .AddHttpMessageHandler(sp => sp.GetRequiredService<AuthorizationMessageHandler>()
    .ConfigureHandler(
        authorizedUrls: new[] { gameStoreApiUrl },
        scopes: new[] { "games:write" }));

builder.Services.AddAuth0OidcAuthentication(options =>
{
    // Configure your authentication provider options here.
    // For more information, see https://aka.ms/blazor-standalone-auth
    builder.Configuration.Bind("Auth0", options.ProviderOptions);
    options.ProviderOptions.AdditionalProviderParameters.Add("audience", audience);
    options.ProviderOptions.MetadataSeed.EndSessionEndpoint = $"{authority}/v2/logout?client_id={clientId}&returnTo={builder.HostEnvironment.BaseAddress}";
})
.AddAccountClaimsPrincipalFactory<ArrayClaimsPrincipalFactory>();

await builder.Build().RunAsync();

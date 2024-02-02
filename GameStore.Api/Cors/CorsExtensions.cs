namespace GameStore.Api.Cors;

public static class CorsExtensions
{
    private const string allowedOriginSettings = "AllowedOrigin";

    public static IServiceCollection AddGameStoreCors(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        return services.AddCors(options =>
        {
            options.AddDefaultPolicy(corsBuilder =>
            {
                var allowedOrigin = configuration[allowedOriginSettings] 
                                    ?? throw new InvalidOperationException("AllowedOrigin is not set");
                corsBuilder.WithOrigins(allowedOrigin)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .WithExposedHeaders("X-Pagination");
            });
        });
    }
}
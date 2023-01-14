using Microsoft.Extensions.DependencyInjection;

namespace BytexDigital.Blazor.Components.GoogleAnalytics;

public static class GoogleAnalyticsExtensions
{
    public static IServiceCollection AddGoogleAnalytics(
        this IServiceCollection services,
        Action<GoogleAnalyticsOptions> configure)
    {
        services.AddOptions<GoogleAnalyticsOptions>().Configure(configure);
        services.AddScoped<GoogleAnalyticsService>();

        return services;
    }
}
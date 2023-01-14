namespace BytexDigital.Blazor.Components.GoogleAnalytics;

public class GoogleAnalyticsOptions
{
    /// <summary>
    /// If set to true, automatically loads the required JS from Google on app startup. Should be disabled when running in a GDPR compliant scenario.
    /// If disabled, tracking can be initialized by calling <see cref="BytexDigital.Blazor.Components.GoogleAnalytics.GoogleAnalyticsService.InitializeAsync"/>
    /// </summary>
    public bool AutomaticallyInitialize { get; set; } = true;

    /// <summary>
    /// If set to true, tracking of events such as changing pages is automatically enabled after initialization has been performed.
    /// </summary>
    public bool AutomaticallyEnableTracking { get; set; } = true;
    
    /// <summary>
    /// Google provided analytics ID.
    /// </summary>
    public string TrackingId { get; set; }
    
    /// <summary>
    /// Configuration data that is supplied to JS on initialization of Google Tag Manager. 
    /// </summary>
    public Dictionary<string, object> GlobalConfigData { get; set; } = new();
    
    /// <summary>
    /// Keys with values that are automatically appended to all dispatched tracking events.
    /// </summary>
    public Dictionary<string, object> GlobalEventData { get; set; } = new();
}
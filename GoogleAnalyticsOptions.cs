namespace BytexDigital.Blazor.Components.GoogleAnalytics;

public class GoogleAnalyticsOptions
{
    public bool AutomaticallyInitialize { get; set; }
    public bool AutomaticallyEnableTracking { get; set; }
    public string TrackingId { get; set; }
    public Dictionary<string, object> GlobalConfigData { get; set; } = new();
    public Dictionary<string, object> GlobalEventData { get; set; } = new();
}
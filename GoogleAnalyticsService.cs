using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;

namespace BytexDigital.Blazor.Components.GoogleAnalytics;

public class GoogleAnalyticsService
{
    private readonly IJSRuntime _js;
    private readonly IOptions<GoogleAnalyticsOptions> _options;
    private readonly ILogger<GoogleAnalyticsService> _logger;
    private bool _isInitialized;
    public bool IsTrackingEnabled { get; set; } = true;

    public GoogleAnalyticsService(IJSRuntime js, IOptions<GoogleAnalyticsOptions> options, ILogger<GoogleAnalyticsService> logger)
    {
        _js = js;
        _options = options;
        _logger = logger;
    }

    public async Task AppStartedAsync(CancellationToken cancellationToken = default)
    {
        _logger.LogDebug($"Received app started event.");
        
        if (_options.Value.AutomaticallyInitialize)
        {
            await InitializeAsync(cancellationToken);
        }

        IsTrackingEnabled = _options.Value.AutomaticallyEnableTracking;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken)
    {
        if (_isInitialized) return;

        try
        {
            _logger.LogDebug($"Initializing.");

            await _js.InvokeVoidAsync(
                GoogleAnalyticsInteropConstants.Configure,
                cancellationToken,
                _options.Value.TrackingId,
                _options.Value.GlobalConfigData);

            _isInitialized = true;
            _logger.LogDebug($"Initialized!");
        }
        catch
        {
            _logger.LogWarning($"Could not initialize! JS interop may be blocked or JS is unavailable.");
        }
    }

    public Task TrackEventAsync(
        string eventName,
        string eventCategory = null,
        string eventLabel = null,
        int? eventValue = null,
        CancellationToken cancellationToken = default)
    {
        return TrackEventAsync(eventName,
            new
            {
                event_category = eventCategory,
                event_label = eventLabel,
                value = eventValue
            },
            cancellationToken);
    }

    public async Task TrackEventAsync(string eventName, object eventData, CancellationToken cancellationToken = default)
    {
        if (!IsTrackingEnabled)
        {
            _logger.LogDebug($"Received request to track event, but tracking is disabled, discarding.");
            return;
        }

        _logger.LogDebug("Tracking event {EventName} ({EventData}).", eventName, JsonSerializer.Serialize(eventData));

        try
        {
            await _js.InvokeVoidAsync(GoogleAnalyticsInteropConstants.TrackEvent,
                cancellationToken,
                eventName,
                eventData,
                _options.Value.GlobalEventData);
        }
        catch
        {
            _logger.LogWarning($"Could not track event! JS interop may be blocked or JS is unavailable.");
        }
    }

    public async Task TrackNavigationAsync(string uri, CancellationToken cancellationToken = default)
    {
        if (!IsTrackingEnabled)
        {
            _logger.LogDebug($"Received request to track navigation, but tracking is disabled, discarding.");
            return;
        }

        _logger.LogDebug("Tracking navigation to {Uri}.", uri);

        try
        {
            await _js.InvokeVoidAsync(
                GoogleAnalyticsInteropConstants.Navigate,
                cancellationToken,
                _options.Value.TrackingId,
                uri);
        }
        catch
        {
            _logger.LogWarning($"Could not track navigation! JS interop may be blocked or JS is unavailable.");
        }
    }
}
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Routing;

namespace BytexDigital.Blazor.Components.GoogleAnalytics;

public class NavigationAnalyticsTracker : ComponentBase
{
    [Inject]
    protected GoogleAnalyticsService Service { get; set; }

    [Inject]
    protected NavigationManager NavigationManager { get; set; }

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();

        NavigationManager.LocationChanged += OnLocationChanged;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        await base.OnAfterRenderAsync(firstRender);

        if (firstRender)
        {
            await Service.AppStartedAsync();
            await OnLocationChanged(NavigationManager.Uri);
        }
    }

    private async void OnLocationChanged(object sender, LocationChangedEventArgs args)
    {
        await OnLocationChanged(args.Location);
    }

    private async Task OnLocationChanged(string location)
    {
        await Service.TrackNavigationAsync(location);
    }

    public void Dispose()
    {
        NavigationManager.LocationChanged -= OnLocationChanged;
    }
}
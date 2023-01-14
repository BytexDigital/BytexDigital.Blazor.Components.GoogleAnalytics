# GoogleAnalytics
This library adds a simple way to integrate Google Analytics in your Blazor WASM/Serverside app. Make your integration of Google Analytics GDPR compliant by using our [BytexDigital.Blazor.Components.CookieConsent](https://github.com/BytexDigital/BytexDigital.Blazor.Components.CookieConsent) library.

## How to install
```ps1
Install-Package BytexDigital.Blazor.Components.GoogleAnalytics
```

<br />

### Requirements
.NET >= 5.0

<br />

### Configure in your project

#### 1. Configure your App.razor
Beneath

```csharp
</Router>
```

add

```csharp
<BytexDigital.Blazor.Components.GoogleAnalytics.NavigationAnalyticsTracker />
```

#### 2. Configure your Program.cs
Add the following call with your desired settings.

```csharp
builder.Services.AddGoogleAnalytics(options =>
{
    options.TrackingId = "G-YOUR_ID";
    options.AutomaticallyInitialize = true;
    options.AutomaticallyEnableTracking = true;
});
```

#### 3. Include necessary JavaScript
Inside your index.html/_Host.cshtml, add the required JS include where you see fit.
```html
<script src="_content/BytexDigital.Blazor.Components.GoogleAnalytics/Analytics.js"></script>
```

## Usage together with [BytexDigital.Blazor.Components.CookieConsent](https://github.com/BytexDigital/BytexDigital.Blazor.Components.CookieConsent)
Using [BytexDigital.Blazor.Components.CookieConsent](https://github.com/BytexDigital/BytexDigital.Blazor.Components.CookieConsent), you can easily make the usage of Google Analytics in your app GDPR compliant by delaying initialization of Google Analytics until user consent for tracking has been given.

To enable this scenario, you should first disable automatic initialization:

```csharp
builder.Services.AddGoogleAnalytics(options =>
{
    options.TrackingId = "G-YOUR_ID";
    options.AutomaticallyInitialize = false; // Disabled
    options.AutomaticallyEnableTracking = true;
});
```

After installing the CookieConsent library and following it's installation instructions, you can add a "Google Analytics" category, which the user has to give consent to for Google Analytics to load and start tracking user behaviour on the website.

The category could be added as follows with the name `analytics`:

```csharp
builder.Services.AddCookieConsent(o =>
{
    // .. Other non related configuration omitted
    
    // 
    o.Categories.Add(new CookieCategory
    {
        Identifier = "analytics", // Unique name for this category
        IsPreselected = false, // In a GDPR compliant scenario, non-essential categories may not be preselected, hence, this has to be false.
    
        TitleText = new Dictionary<string, string>
        {
            ["en"] = "Analytics"
        },
        DescriptionText = new Dictionary<string, string>
        {
            ["en"] = "Allows the integration and usage of Google Analytics to track user behavior on our website."
        },

        Services = new List<CookieCategoryService>
        {
            new()
            {
                Identifier = "analytics",
                PolicyUrl = "https://policies.google.com/privacy",
                TitleText = new Dictionary<string, string>
                {
                    ["en"] = "Google Analytics"
                },
                ShowPolicyText = new Dictionary<string, string>
                {
                    ["en"] = "Show policy"
                }
            }
        }
    });
});
```

Inside your App.razor, you can now listen for the CookieConsent library to broadcast the event that the user has changed his cookie preferences and use the given preferences to determine whether the GoogleAnalytics library can now initialize and begin tracking, for example as follows:

```cs
// Prior RazorHMTL content of your App.razor

@code {

    [Inject]
    public GoogleAnalyticsService GoogleAnalyticsService { get; set; }

    [Inject]
    public CookieConsentService CookieConsentService { get; set; }

    protected override void OnInitialized()
    {
        // Subscribe to the CookiePreferencesChanged event so we can react to given consent.
        CookieConsentService.CookiePreferencesChanged += async (sender, preferences) => await OnPreferencesChangedAsync();
    }

    protected async Task OnPreferencesChangedAsync()
    {
        // Fetch the now active cookie preferences.
        var preferences = await CookieConsentService.GetPreferencesAsync();

        if (preferences.AllowedServices.Contains("analytics"))
        {
            // If analytics are allowed by user consent, initialize GoogleAnalytics.
            await GoogleAnalyticsService.InitializeAsync(default);
        }
    }

}
```

# Changelog

### 1.0.0

<details>
  <summary>Click to expand!</summary>

   <br /> 

- Initial release
</details>

# Licensed content

This library contains code from the following sources.
```cs
/*

https://github.com/isc30/blazor-analytics
MIT License
Copyright (c) 2019 Ivan Sanz Carasa

*/
```

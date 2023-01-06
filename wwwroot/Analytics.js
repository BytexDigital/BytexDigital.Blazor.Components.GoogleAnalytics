/*

https://github.com/isc30/blazor-analytics
MIT License
Copyright (c) 2019 Ivan Sanz Carasa

*/

window.dataLayer = window.dataLayer || [];
window.gtag = window.gtag || function () { dataLayer.push(arguments); };
gtag("js", new Date());

var BlazorGoogleAnalytics;

(function (BlazorGoogleAnalytics) {
    function configure(trackingId, globalConfigObject) {
        this.globalConfigObject = globalConfigObject;
        
        var script = document.createElement("script");
        script.async = true;
        script.src = "https://www.googletagmanager.com/gtag/js?id=" + trackingId;
        document.head.appendChild(script);
        
        var configObject = {};
        configObject.send_page_view = false;
        Object.assign(configObject, globalConfigObject);
        
        gtag("config", trackingId, configObject);
    }
    
    BlazorGoogleAnalytics.configure = configure;
    
    function navigate(trackingId, href) {
        var configObject = {};
        configObject.page_location = href;
        Object.assign(configObject, this.globalConfigObject);
        
        gtag("config", trackingId, configObject);
    }
    
    BlazorGoogleAnalytics.navigate = navigate;
    
    function trackEvent(eventName, eventData, globalEventData) {
        Object.assign(eventData, globalEventData);
        gtag("event", eventName, eventData);
    }
    
    BlazorGoogleAnalytics.trackEvent = trackEvent;
})(BlazorGoogleAnalytics || (BlazorGoogleAnalytics = {}));
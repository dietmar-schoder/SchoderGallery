using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SchoderGallery;
using SchoderGallery.Builders;
using SchoderGallery.Settings;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<ISettingsFactory, SettingsFactory>();
builder.Services.AddScoped<ISettings, PortraitSettings>();
builder.Services.AddScoped<ISettings, LandscapeSettings>();

builder.Services.AddScoped<IFacadeBuilder, FacadeBuilder>();

await builder.Build().RunAsync();

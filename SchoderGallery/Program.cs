using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SchoderGallery;
using SchoderGallery.Algorithms;
using SchoderGallery.Builders;
using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Settings;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<ColourGenerator>();
builder.Services.AddSingleton<NavigationService>();
builder.Services.AddSingleton<SvgPainter>();

builder.Services.AddScoped<SettingsFactory>();
builder.Services.AddScoped<ISettings, LandscapeSettings>();
builder.Services.AddScoped<ISettings, PortraitSettings>();

builder.Services.AddScoped<BuilderFactory>();
builder.Services.AddScoped<IBuilder, FacadeBuilder>();
builder.Services.AddScoped<IBuilder, Floor1Builder>();
builder.Services.AddScoped<IBuilder, GroundFloorBuilder>();
builder.Services.AddScoped<IBuilder, LiftBuilder>();

await builder.Build().RunAsync();
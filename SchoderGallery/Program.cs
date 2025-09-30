using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SchoderGallery;
using SchoderGallery.Builders;
using SchoderGallery.Constants;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddScoped<IConstantsFactory, ConstantsFactory>();
builder.Services.AddScoped<IConstants, PortraitConstants>();
builder.Services.AddScoped<IConstants, LandscapeConstants>();

builder.Services.AddScoped<IFacadeBuilder, FacadeBuilder>();

await builder.Build().RunAsync();

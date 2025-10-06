using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SchoderGallery;
using SchoderGallery.Algorithms;
using SchoderGallery.Builders;
using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Services;
using SchoderGallery.Settings;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<ColourGenerator>();
builder.Services.AddSingleton<SvgPainter>();
builder.Services.AddSingleton<IGalleryService, GalleryService>();

builder.Services.AddScoped<NavigationService>();

builder.Services.AddScoped<SettingsFactory>();
builder.Services.AddScoped<ISettings, LandscapeSettings>();
builder.Services.AddScoped<ISettings, PortraitSettings>();

builder.Services.AddSingleton<AlgorithmFactory>();
builder.Services.AddSingleton<IAlgorithm, TurtleGraphics>();
builder.Services.AddSingleton<IAlgorithm, FourColours>();

builder.Services.AddScoped<BuilderFactory>();
builder.Services.AddScoped<IArtworkBuilder, ArtworkBuilder>();
builder.Services.AddScoped<IBuilder, AtelierBuilder>();
builder.Services.AddScoped<IBuilder, Basement1Builder>();
builder.Services.AddScoped<IBuilder, Basement2Builder>();
builder.Services.AddScoped<IBuilder, DepotBuilder>();
builder.Services.AddScoped<IBuilder, FacadeBuilder>();
builder.Services.AddScoped<IBuilder, Floor1Builder>();
builder.Services.AddScoped<IBuilder, Floor2Builder>();
builder.Services.AddScoped<IBuilder, Floor3Builder>();
builder.Services.AddScoped<IBuilder, Floor4Builder>();
builder.Services.AddScoped<IBuilder, Floor5Builder>();
builder.Services.AddScoped<IBuilder, Floor6Builder>();
builder.Services.AddScoped<IBuilder, GroundFloorBuilder>();
builder.Services.AddScoped<IBuilder, LiftBuilder>();
builder.Services.AddScoped<IBuilder, OperationsBuilder>();

await builder.Build().RunAsync();
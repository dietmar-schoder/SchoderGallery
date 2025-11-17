using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SchoderGallery;
using SchoderGallery.Algorithms;
using SchoderGallery.Builders;
using SchoderGallery.Helpers;
using SchoderGallery.Navigation;
using SchoderGallery.Painters;
using SchoderGallery.Services;
using SchoderGallery.Settings;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddHttpClient(Const.Backend, client => { client.BaseAddress = new Uri(Const.SchoderGalleryServerUrl); });
builder.Services.AddScoped<ClientFactory>();
builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();

builder.Services.AddSingleton<Colours>();
builder.Services.AddSingleton<Image>();
builder.Services.AddSingleton<SvgPainter>();

builder.Services.AddScoped<GalleryService>();
builder.Services.AddScoped<NavigationService>();

builder.Services.AddScoped<SettingsFactory>();
builder.Services.AddScoped<ISettings, LandscapeSettings>();
builder.Services.AddScoped<ISettings, PortraitSettings>();

builder.Services.AddSingleton<AlgorithmFactory>();
builder.Services.AddSingleton<IAlgorithm, FourColours>();
builder.Services.AddSingleton<IAlgorithm, Image>();
builder.Services.AddSingleton<IAlgorithm, TurtleGraphics>();

builder.Services.AddSingleton<SizeHelperFactory>();
builder.Services.AddSingleton<ISizeHelper, DynamicSizeHelper>();
builder.Services.AddSingleton<ISizeHelper, FixedSizeHelper>();
builder.Services.AddSingleton<ISizeHelper, RatioSizeHelper>();
builder.Services.AddSingleton<ISizeHelper, FixedPortLandSizeHelper>();
builder.Services.AddSingleton<ISizeHelper, TextSizeHelper>();

builder.Services.AddScoped<BuilderFactory>();
builder.Services.AddScoped<IArtworkInfoBuilder, ArtworkInfoBuilder>();
builder.Services.AddScoped<IArtworkBuilder, ArtworkBuilder>();
builder.Services.AddScoped<IBuilder, AtelierBuilder>();
builder.Services.AddScoped<IBuilder, Basement1Builder>();
builder.Services.AddScoped<IBuilder, Basement2Builder>();
builder.Services.AddScoped<IBuilder, CafeBuilder>();
builder.Services.AddScoped<IBuilder, DepotBuilder>();
builder.Services.AddScoped<IBuilder, FacadeBuilder>();
builder.Services.AddScoped<IBuilder, Floor1Builder>();
builder.Services.AddScoped<IBuilder, Floor2Builder>();
builder.Services.AddScoped<IBuilder, Floor3Builder>();
builder.Services.AddScoped<IBuilder, Floor4Builder>();
builder.Services.AddScoped<IBuilder, Floor5Builder>();
builder.Services.AddScoped<IBuilder, Floor6Builder>();
builder.Services.AddScoped<IBuilder, GroundFloorBuilder>();
builder.Services.AddScoped<IBuilder, InfoBuilder>();
builder.Services.AddScoped<IBuilder, LiftBuilder>();
builder.Services.AddScoped<IBuilder, MyCollectionBuilder>();
builder.Services.AddScoped<IBuilder, ShopBuilder>();
builder.Services.AddScoped<IBuilder, ToiletsBuilder>();

await builder.Build().RunAsync();

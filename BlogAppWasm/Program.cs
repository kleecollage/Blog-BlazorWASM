using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlogAppWasm;
using BlogAppWasm.Services;
using BlogAppWasm.Services.IService;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
// MY SERVICES
builder.Services.AddScoped<IPostService, PostService>();
// LOCALSTORAGE
builder.Services.AddBlazoredLocalStorage();
// AUTHENTICATION AND AUTHORIZATION
builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<AuthStateProvider>());

await builder.Build().RunAsync();
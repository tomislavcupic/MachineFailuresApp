using MachineFailures.Blazor.Components;
using System.Net.Http;
using MachineFailures.Blazor.Components.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient("api", client =>
{
    client.BaseAddress = new Uri("http://localhost:5246/");
});

builder.Services.AddScoped<ApiService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();

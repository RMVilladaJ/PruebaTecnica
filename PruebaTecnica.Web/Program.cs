using CurrieTechnologies.Razor.SweetAlert2;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using PruebaTecnica.Web;
using PruebaTecnica.Web.Repositories;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7000") }); //URL para consumir el API
builder.Services.AddScoped<IRepository, Repository>(); //Inyecci�n Repository
builder.Services.AddSweetAlert2();

await builder.Build().RunAsync();

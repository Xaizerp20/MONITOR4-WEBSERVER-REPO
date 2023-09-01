using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Monitor4WebServer.Data;
using Monitor4WebServer.Services;
using Toolbelt.Blazor.Extensions.DependencyInjection;
using Syncfusion.Blazor;
using Microsoft.JSInterop;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using MudBlazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
builder.Services.AddSyncfusionBlazor();
builder.Services.AddSingleton<MQTTService>();
builder.Services.AddScoped<LocalDb>();
builder.Services.AddScoped<LineReportDbService>();
builder.Services.AddSpeechSynthesis();
builder.Services.AddScoped<NotificationService>();
builder.Logging.SetMinimumLevel(LogLevel.Warning);
builder.Services.AddMudServices();

/*builder.Services.AddDbContext<LineReportContext>(options =>
    options.UseSqlite(@"Data Source=C:\Users\Informatica\Desktop\MONITOR4DBSQLITE\LineReport.db"));*/

var app = builder.Build();

//Register Syncfusion license
Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1NGaF5cXmdCeUx0TXxbf1xzZFZMYF9bR39PIiBoS35RdUVlW3dfdnVRQmleVUV2");


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

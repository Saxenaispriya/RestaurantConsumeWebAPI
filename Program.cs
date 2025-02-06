using Microsoft.AspNetCore.HttpLogging;
using RestaurantConsumeWebAPI.Controllers;
using System.Net.Security;

var builder = WebApplication.CreateBuilder(args);

// Register HttpClient with base address
builder.Services.AddHttpClient<RestaurantConsumeController>(client =>
{
    //client.BaseAddress = new Uri("http://localhost:5236/api/");
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpLogging(logging =>
{
    // Customize HTTP logging here.
    logging.LoggingFields = HttpLoggingFields.All;
    logging.RequestHeaders.Add("sec-ch-ua");
    logging.ResponseHeaders.Add("my-response-header");
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpLogging();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=RestaurantConsume}/{action=Index}/{id?}");

app.Run();

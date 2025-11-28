using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ScribeTrax.Context;
using ScribeTrax.Interfaces;
using ScribeTrax.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<ScribeTraxDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ScribeTraxDb")));

builder.Services.AddScoped<IBylineService, BylineService>();
builder.Services.AddScoped<IWorkService, WorkService>();
builder.Services.AddScoped<IMarketService, MarketService>();
builder.Services.AddScoped<ISubmissionService, SubmissionService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

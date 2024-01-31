using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using SoftDeleteMultilingualBaseDbContext.Localization;
using SoftDeleteMultilingualBaseDbContext.Middlewares;
using SoftDeleteMultilingualBaseDbContext.Repository.Contexts;
using System.Globalization;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddRazorPages();

        builder.Services.AddLocalization(options =>
            options.ResourcesPath = "Resources");

        builder.Services.AddScoped<ISharedResource, SharedResource>();

        builder.Services.AddScoped<RequestLocalizationCookiesMiddleware>();

        builder.Services.Configure<RequestLocalizationOptions>(options =>
        {
            var supportedCultures = new CultureInfo[] { new("tr-TR"), new("en-US"), new("fr-FR") };

            options.DefaultRequestCulture = new RequestCulture("en-US");
            options.SupportedCultures = supportedCultures;
            options.SupportedUICultures = supportedCultures;
        });

        builder.Services.AddDbContext<ADbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("A")));

        builder.Services.AddDbContext<BDbContext>(options =>
            options.UseSqlServer(builder.Configuration.GetConnectionString("B")));


        var app = builder.Build();

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

        app.UseAuthorization();

        app.MapRazorPages();

        app.UseRequestLocalization();
        app.UseRequestLocalizationCookies();

        app.Run();
    }
}
using DAL;
using DAL.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace RealEstate
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddControllersWithViews();




            #region Configure service
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation()
              .AddViewOptions(options =>
              {
                  options.HtmlHelperOptions.ClientValidationEnabled = true;
              });

            builder.Services.AddDbContext<RealEstateContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.AccessDeniedPath = "/Account/AccessDenied";
                options.Cookie.Name = "Cookie";
                options.Cookie.HttpOnly = true;
                options.ExpireTimeSpan = TimeSpan.FromMinutes(720);
                options.LoginPath = "/Account/Login";
                options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
                options.SlidingExpiration = true;
            });




            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<RealEstateContext>().AddDefaultTokenProviders();


            builder.Services.AddAutoMapper(typeof(Program).Assembly); 
            #endregion



            #region Update Database
            // Group of services lifetime scopped
            using var Scope = builder.Services.BuildServiceProvider().CreateScope();
            // Services its self
            var Services = Scope.ServiceProvider;

            var LoggerFactory = Services.GetRequiredService<ILoggerFactory>();
            try
            {

                // Ask CLR for creating object from DbContext Explicitly
                var dbContext = Services.GetRequiredService<RealEstateContext>();
                // Update-database
                await dbContext.Database.MigrateAsync();

                // Data Seeding
                await ContextDataSeed.SeedAsync(dbContext, Services);


                // Refresh site settings from the database
                Services.LoadSettingsFromDatabase(builder.Configuration);



            }
            catch (Exception ex)
            {
                var Logger = LoggerFactory.CreateLogger<Program>();
                Logger.LogError(ex, "An error occured during appling the migration");
            }

            // Bind the updated configuration to the SiteSettings class
            builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection("SiteSettings"));
            //Scope.Dispose();
            #endregion



            var app = builder.Build();

            #region  Configure the HTTP request pipeline. 
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();



            // Add middleware for error handling
            app.UseStatusCodePages(async context =>
            {
                var response = context.HttpContext.Response;

                if (response.StatusCode == 404)
                {
                    response.Redirect("/Error/NotFound");
                }
            });



            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                    name: "Admin",
                    areaName: "Admin",
                    pattern: "Admin/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                   name: "Default",
                   pattern: "{controller=Home}/{action=Index}/{id?}"
                   );
            });


            #endregion
            app.Run();
        }
    }
}

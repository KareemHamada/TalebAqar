using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace RealEstate
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //builder.Services.AddControllersWithViews();




            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation()
	        .AddViewOptions(options =>
	        {
		        options.HtmlHelperOptions.ClientValidationEnabled = true;
	        });

			builder.Services.AddDbContext<RealEstateContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });


            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<RealEstateContext>().AddDefaultTokenProviders();


			builder.Services.AddAutoMapper(typeof(Program).Assembly);


            //// using Access Denied
            //builder.Services.ConfigureApplicationCookie(options =>
            //{
            //    options.AccessDeniedPath = "/Users/AccessDenied";
            //    options.Cookie.Name = "Cookie";
            //    options.Cookie.HttpOnly = true;
            //    options.ExpireTimeSpan = TimeSpan.FromMinutes(720);
            //    options.LoginPath = "/Account/Login";
            //    options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
            //    options.SlidingExpiration = true;
            //});
            //// end of Access Denied


            


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


            app.Run();
        }
    }
}

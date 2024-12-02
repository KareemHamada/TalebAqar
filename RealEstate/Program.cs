using DAL.Models;
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


            // Retrieve logo path from the database
            using (var scope = builder.Services.BuildServiceProvider().CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<RealEstateContext>();
                var settingsData = context.TbSettings.FirstOrDefault();

                builder.Configuration["SiteSettings:Logo"] = settingsData?.Logo ?? "/Uploads/DefaultImages/logo.jpg";
                builder.Configuration["SiteSettings:WebsiteName"] = settingsData?.WebsiteName;
                builder.Configuration["SiteSettings:WebsiteDescription"] = settingsData?.WebsiteDescription;
                builder.Configuration["SiteSettings:FacebookLink"] = settingsData?.FacebookLink;
                builder.Configuration["SiteSettings:TwitterLink"] = settingsData?.TwitterLink;
                builder.Configuration["SiteSettings:InstgramLink"] = settingsData?.InstgramLink;
                builder.Configuration["SiteSettings:LinkedinLink"] = settingsData?.LinkedinLink;
                builder.Configuration["SiteSettings:YoutubeLink"] = settingsData?.YoutubeLink;
                builder.Configuration["SiteSettings:Address"] = settingsData?.Address;
                builder.Configuration["SiteSettings:ContactNumber"] = settingsData?.ContactNumber;
                builder.Configuration["SiteSettings:Email"] = settingsData?.Email;
                builder.Configuration["SiteSettings:MainPanner"] = settingsData?.MainPanner;
                builder.Configuration["SiteSettings:PropertyDetailsPanner"] = settingsData?.PropertyDetailsPanner;

            }
            builder.Services.Configure<SiteSettings>(builder.Configuration.GetSection("SiteSettings"));





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

namespace RealEstate.Utilities
{
    public static class ConfigurationExtensions
    {
        public static void LoadSettingsFromDatabase(this IServiceProvider services, IConfiguration configuration)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<RealEstateContext>();
            var settingsData = context.TbSettings.FirstOrDefault();

            // Update configuration dynamically
            if (settingsData != null)
            {
                configuration["SiteSettings:Logo"] = settingsData?.Logo ?? "";
                configuration["SiteSettings:WebsiteName"] = settingsData?.WebsiteName ?? "";
                configuration["SiteSettings:WebsiteDescription"] = settingsData?.WebsiteDescription ?? "";
                configuration["SiteSettings:FacebookLink"] = settingsData?.FacebookLink ?? "";
                configuration["SiteSettings:TwitterLink"] = settingsData?.TwitterLink ?? "";
                configuration["SiteSettings:InstgramLink"] = settingsData?.InstgramLink ?? "";
                configuration["SiteSettings:LinkedinLink"] = settingsData?.LinkedinLink ?? "";
                configuration["SiteSettings:YoutubeLink"] = settingsData?.YoutubeLink ?? "";
                configuration["SiteSettings:Address"] = settingsData?.Address ?? "";
                configuration["SiteSettings:ContactNumber"] = settingsData?.ContactNumber ?? "";
                configuration["SiteSettings:Email"] = settingsData?.Email ?? "";
                configuration["SiteSettings:MainPanner"] = settingsData?.MainPanner ?? "";
                configuration["SiteSettings:PropertyDetailsPanner"] = settingsData?.PropertyDetailsPanner ?? "";
            }
                
        }
    }
}

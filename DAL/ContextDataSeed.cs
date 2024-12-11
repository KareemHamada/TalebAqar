namespace DAL
{
    public static class ContextDataSeed
    {
        // Seeding
        public static async Task SeedAsync(RealEstateContext dbContext, IServiceProvider serviceProvider)
        {
            try
            {
                // for admin
                var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();


                // Define roles
                var roles = new[] { "Admin", "Data Entry" };

                // Seed roles
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                // Seed admin user
                if (await userManager.FindByEmailAsync("kareemhamada219@gmail.com") == null)
                {
                    var adminUser = new ApplicationUser
                    {
                        UserName = "KareemHamada",
                        Email = "kareemhamada219@gmail.com",
                        FirstName = "Kareem",
                        LastName = "Hamada",
                        EmailConfirmed = true
                    };

                    var result = await userManager.CreateAsync(adminUser, "Kareem289!@#289!@#!");

                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(adminUser, "Admin");
                    }
                }


                // for statuses
                if (!dbContext.TbStatuses.Any())
                {
                    //read statueses from file as string
                    var StatusesData = await File.ReadAllTextAsync("../DAL/Data/DataSeed/statuses.json");

                    // transform into C# objects
                    var Statuses = JsonSerializer.Deserialize<List<TbStatus>>(StatusesData);

                    // add to db & save
                    if (Statuses is not null && Statuses.Any())
                    {
                        await dbContext.TbStatuses.AddRangeAsync(Statuses);
                        await dbContext.SaveChangesAsync();
                    }

                }


                // for statuses
                if (!dbContext.TbSettings.Any())
                {
                    //read statueses from file as string
                    var SettingData = await File.ReadAllTextAsync("../DAL/Data/DataSeed/setting.json");

                    // transform into C# objects
                    var setting = JsonSerializer.Deserialize<List<TbSetting>>(SettingData);

                    // add to db & save
                    if (setting is not null && setting.Any())
                    {
                        await dbContext.TbSettings.AddRangeAsync(setting);
                        await dbContext.SaveChangesAsync();
                    }

                }

            }
            catch (Exception ex) { }
        }
    }
}

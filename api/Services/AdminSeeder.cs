using Microsoft.AspNetCore.Identity;

namespace APIPractice.Services
{
    public class AdminSeeder
    {
        private readonly UserManager<IdentityUser> userManager;

        public AdminSeeder(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task SeedAdminAsync()
        {
            var adminEmail = "admin@shopsy.com";
            var adminPassword = "admin@123";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var result = await userManager.CreateAsync(adminUser, adminPassword);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }
        }
    }
}

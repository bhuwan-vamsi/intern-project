using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace APIPractice.Data
{
    public class ApplicationAuthDbContext : IdentityDbContext
    {
        public ApplicationAuthDbContext(DbContextOptions<ApplicationAuthDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var managerId = "35da3d5d-79f4-44e0-8d6b-08d0dcf75991";
            var employeeId = "9d12498c-de59-46af-a8f2-81e7cee88f7c";
            var customerId = "5ae289a5-da00-4d60-8b1e-4183a019d8c1";
            var adminRoleId = "e10adcf6-d187-45bb-b32b-a040a9023e7d";

            var roles = new List<IdentityRole>
            {
                new IdentityRole
                {
                    Id = managerId,
                    ConcurrencyStamp = managerId,
                    Name = "Manager",
                    NormalizedName = "MANAGER"
                },
                new IdentityRole
                {
                    Id = employeeId,
                    ConcurrencyStamp = employeeId,
                    Name = "Employee",
                    NormalizedName = "EMPLOYEE"
                },
                new IdentityRole
                {
                    Id = customerId,
                    ConcurrencyStamp = customerId,
                    Name = "Customer",
                    NormalizedName = "CUSTOMER"
                },
                new IdentityRole
                {
                    Id = adminRoleId,
                    ConcurrencyStamp = adminRoleId,
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                }
            };

            builder.Entity<IdentityRole>().HasData(roles);

            var adminUserId = "6afa382c-ef11-41ce-9257-24f3a5c0a2ee";
            var adminEmail = "admin@shopsy.com";

            var hasher = new PasswordHasher<IdentityUser>();
            var adminUser = new IdentityUser
            {
                Id = adminUserId,
                UserName = adminEmail,
                NormalizedUserName = adminEmail.ToUpper(),
                Email = adminEmail,
                NormalizedEmail = adminEmail.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = "9ac3e0fc-2c04-406e-ab03-daac28d7872d",
                PasswordHash = hasher.HashPassword(null, "admin@123")
            };

            builder.Entity<IdentityUser>().HasData(adminUser);

            builder.Entity<IdentityUserRole<string>>().HasData(new IdentityUserRole<string>
            {
                UserId = adminUserId,
                RoleId = adminRoleId
            });
        }
    }
}

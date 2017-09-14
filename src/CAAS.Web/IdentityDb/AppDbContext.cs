using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using CAAS.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Design;

namespace CAAS
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            this.Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }

        public static async Task CreateAuthorRole(IServiceProvider container)
        {
            using (var serviceScope = container.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                RoleManager<IdentityRole> roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                if (await roleManager.FindByNameAsync("Authors") == null)
                {
                    IdentityResult result = await roleManager.CreateAsync(new IdentityRole("Authors"));
                    if (!result.Succeeded)
                    {
                        throw new Exception("Error creating authors role!");
                    }
                }
            }
        }
        public static async Task CreateAdminAccount(IServiceProvider container, IConfiguration configuration)
        {
            using (var serviceScope = container.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                UserManager<ApplicationUser> userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
                RoleManager<IdentityRole> roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();

                string userName = configuration["Data:AdminUser:Name"];
                string email = configuration["Data:AdminUser:Email"];
                string password = configuration["Data:Adminuser:Password"];
                string role = configuration["Data:AdminUser:Role"];

                if (await userManager.FindByNameAsync(userName) == null)
                {
                    if (await roleManager.FindByNameAsync(role) == null)
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                    ApplicationUser user = new ApplicationUser
                    {
                        UserName = userName,
                        Email = email
                    };
                    IdentityResult result = await userManager.CreateAsync(user, password);
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, role);
                    }
                }
            }
        }
    }

    class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var confBuilder = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets<ApplicationDbContext>();

            IConfiguration Configuration = confBuilder.Build();

            // This is for a mutli-tenant Environment, so the ConnectionString Env Var name can be set
            // In AppSettings.json
            var ConnectionString = Configuration.GetConnectionString("DefaultConnection");

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            //"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Initial Catalog=C1;AttachDbFilename=%CONTENTROOTPATH%\\Data\\C1.mdf;Trusted_Connection=true;MultipleActiveResultSets=true"

            builder.UseSqlServer(@"Data Source =.\SQLEXPRESS; Initial Catalog = CAAS; Trusted_Connection = True; ");

            return new ApplicationDbContext(builder.Options);
        }
    }
}

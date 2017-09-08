using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Bagombo.EFCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Bagombo;

namespace Bagombo.Data
{
    /// <summary>
    /// The whole point of this class is to allow for EF Core code to exist in a seperate project from the web application
    /// and be able to do migrations and so forth
    /// dotnet ef migrations add test1 --startup-project ..\blog
    /// or
    /// https://github.com/aspnet/EntityFramework/issues/7889   -- using this fix
    /// https://docs.microsoft.com/en-us/ef/core/miscellaneous/configuring-dbcontext
    /// </summary>
    /// 
    class StartupForEF : IDbContextFactory<BlogDbContext>
    {
        public BlogDbContext Create(DbContextFactoryOptions options)
        {
            var confBuilder = new ConfigurationBuilder()
              .AddEnvironmentVariables()
              .AddUserSecrets<StartupForEF>();

            IConfiguration Configuration = confBuilder.Build();

            // This is for a mutli-tenant Environment, so the ConnectionString Env Var name can be set
            // In AppSettings.json
            var ConnectionString = Configuration.GetConnectionString("DefaultConnection");

            var builder = new DbContextOptionsBuilder<BlogDbContext>();

            builder.UseSqlServer(@"Data Source =.\SQLEXPRESS; Initial Catalog = CAAS; Trusted_Connection = True; ");

            return new BlogDbContext(builder.Options);

        }
    }
}

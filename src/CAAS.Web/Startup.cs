using CAAS.Controllers;
using CAAS.Data.Command.EFCoreCommandHandlers;
using CAAS.Data.Query.EFCoreQueryHandlers;
using CAAS.EFCore;
using CAAS.Models;
using Microsoft.AspNetCore.Authentication.Facebook;
using Microsoft.AspNetCore.Authentication.Twitter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SimpleInjector;
using SimpleInjector.Integration.AspNetCore.Mvc;
using SimpleInjector.Lifestyles;
using System;

namespace CAAS
{
    public class Startup
    {
        IConfiguration Configuration;
        private Container _container = new Container();
        private string _contentRootPath = "";

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();

            if (env.IsDevelopment())
            {
                builder.AddUserSecrets<Startup>();
            }

            Configuration = builder.Build();

            _contentRootPath = env.ContentRootPath;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            //"DefaultConnection": "Server=(localdb)\\mssqllocaldb;Initial Catalog=C1;AttachDbFilename=%CONTENTROOTPATH%\\Data\\C1.mdf;Trusted_Connection=true;MultipleActiveResultSets=true"

            // This is for a mutli-tenant Environment, so the ConnectionString Env Var name can be set
            // In AppSettings.json
            var ConnectionString = Configuration.GetConnectionString("DefaultConnection");
            //if (ConnectionString.Contains("%CONTENTROOTPATH%"))
            //{
            //    ConnectionString = ConnectionString.Replace("%CONTENTROOTPATH%", _contentRootPath);
            //}

            if (ConnectionString == null)
            {
                throw new System.Exception("Unable to determine the Connection String to the database.");
            }

            services.Configure<CAASSettings>(Configuration.GetSection("CAASSettings"));

            services.AddSession();

            services.AddMvc().AddJsonOptions(options =>
            {
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
            });

            services.AddDbContext<BlogDbContext>(options =>
            {
                options.UseSqlServer(ConnectionString);
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                       options.UseSqlServer(ConnectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>(opts =>
            {
                opts.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>()
              .AddDefaultTokenProviders();

            var TwitterKey = Configuration[$"{Configuration["TwitterKeyConfigName"]}"];
            var TwitterSecret = Configuration[$"{Configuration["TwitterSecretConfigName"]}"];

            if (TwitterKey != null && TwitterSecret != null)
            {
                services.AddAuthentication().AddTwitter(twitterOptions =>
                {
                    twitterOptions.ConsumerKey = TwitterKey;
                    twitterOptions.ConsumerSecret = TwitterSecret;
                });
            }

            var FacebookAppId = Configuration[$"{Configuration["FacebookAppIdConfigName"]}"];
            var FacebookAppSecret = Configuration[$"{Configuration["FacebookAppSecretConfigName"]}"];

            services.AddAuthentication().AddFacebook(facebookOptions =>
            {
                facebookOptions.AppId = FacebookAppId;
                facebookOptions.AppSecret = FacebookAppSecret;
            });

            IntegrateSimpleInjector(services);
        }

        public void IntegrateSimpleInjector(IServiceCollection services)
        {
            _container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddSingleton<IControllerActivator>(new SimpleInjectorControllerActivator(_container));
            services.AddSingleton<IViewComponentActivator>(new SimpleInjectorViewComponentActivator(_container));

            services.EnableSimpleInjectorCrossWiring(_container);
            services.UseSimpleInjectorAspNetRequestScoping(_container);
            services.AddSimpleInjectorTagHelperActivation(_container);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {

            InitializeContainer(app);

            _container.Verify();

            // Able to move this after verify after discovering how to create the scoped instances correctly
            // See - https://github.com/aspnet/EntityFramework/issues/5096  and
            // https://github.com/simpleinjector/SimpleInjector/issues/398

            ApplicationDbContext.CreateAdminAccount(app.ApplicationServices, Configuration).Wait();

            ApplicationDbContext.CreateAuthorRole(app.ApplicationServices).Wait();

            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddFile(Configuration.GetSection("Logging"));

            app.UseStaticFiles();

            app.UseSession();

            if (env.IsDevelopment())
            {
                app.UseStatusCodePages();
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseMvcWithDefaultRoute();

        }

        private void InitializeContainer(IApplicationBuilder app)
        {

            // Add application presentation components:
            _container.RegisterMvcControllers(app);
            _container.RegisterMvcViewComponents(app);

            // Cross-wire ASP.NET services (if any). For instance:
            _container.RegisterSingleton(app.ApplicationServices.GetService<ILoggerFactory>());

            _container.CrossWire<BlogDbContext>(app);
            _container.CrossWire<ApplicationDbContext>(app);
            _container.CrossWire<UserManager<ApplicationUser>>(app);
            _container.CrossWire<RoleManager<IdentityRole>>(app);
            _container.CrossWire<SignInManager<ApplicationUser>>(app);
            _container.CrossWire<IPasswordHasher<ApplicationUser>>(app);
            _container.CrossWire<IPasswordValidator<ApplicationUser>>(app);
            _container.CrossWire<IUserValidator<ApplicationUser>>(app);
            _container.CrossWire<ILogger<HomeController>>(app);
            _container.CrossWire<ILogger<AccountController>>(app);
            _container.CrossWire<ILogger<AdminController>>(app);
            _container.CrossWire<ILogger<AuthorController>>(app);

            _container.AddEFQueries();
            _container.AddEFCommands();

            // NOTE: Do prevent cross-wired instances as much as possible.
            // See: https://simpleinjector.org/blog/2016/07/
        }
    }
}

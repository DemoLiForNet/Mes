using System;
using System.Reflection;
using Mes.API.Configuration;
using Mes.Core.Domain;
using Mes.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mes.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            services.AddDbContext<MesDbContext>(options =>
             options.UseSqlServer(Configuration.GetConnectionString("IdentityConnectionString"),
                                     sqlServerOptionsAction: sqlOptions =>
                                     {
                                         sqlOptions.MigrationsAssembly(typeof(BaseEntityTypeConfiguration<>).GetTypeInfo().Assembly.GetName().Name);
                                         sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                                     }));
            services.AddIdentityServer()
                    .AddInMemoryIdentityResources(Config.GetResources())
                    .AddInMemoryApiResources(Config.GetApiResources())
                    .AddInMemoryClients(Config.GetClients());
            services.AddIdentity<ApplicationUser, ApplicationRole>(options =>
                    {
                        options.SignIn = new SignInOptions
                        {
                            RequireConfirmedPhoneNumber = true
                        };
                        options.Lockout = new LockoutOptions
                        {
                            AllowedForNewUsers = false
                        };
                        options.Password = new PasswordOptions
                        {
                            RequireDigit = false,
                            RequiredUniqueChars = 0,
                            RequireLowercase = false,
                            RequireUppercase = false,
                            RequireNonAlphanumeric = false,
                        };
                    })
                    .AddEntityFrameworkStores<MesDbContext>()
                    .AddDefaultTokenProviders();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                   {
                       options.Authority = Configuration.GetValue<string>("IdentityAuthority"); ;
                       options.RequireHttpsMetadata = false;
                       options.Audience = Configuration.GetValue<string>("ApiName");
                   });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}

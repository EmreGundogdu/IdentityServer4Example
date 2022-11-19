using IdentityServer4.Contrib.Caching.Redis.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace AuthServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddIdentityServer()
                .AddDistributedRedisCache(options =>  // <- this!
                {
                    options.Configuration = "127.0.0.1:6379";
                    options.InstanceName = "my-redis-instance-name";
                },
                options => options.CachingKeyPrefix = "_my-identityserver-caching-prefix_",
                options =>
                {
                    options.LockRetryCount = 1;
                    options.LockRetryDelay = TimeSpan.FromSeconds(1);
                })
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetTestUser().ToList())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddDeveloperSigningCredential(); //clienttan gelen private key'i auth'dan gelen public key ile karþýlaþtýrýr

            services.AddControllersWithViews();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles(); //wwwroot'a eriþim için
            app.UseAuthentication(); //kimlik doðrulama için
            app.UseAuthorization(); //yetkilendirme için

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis;
using System;
using System.Linq;

namespace AuthServer
{
    public class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddIdentityServer()
                .AddOperationalStore(options =>
                {
                    options.RedisConnectionString = "10.10.4.30:6379";
                    options.Db = 0;
                }).AddRedisCaching(options =>
                {
                    options.RedisConnectionString = "10.10.4.30:6379";
                    options.KeyPrefix = "prefix";
                })
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddInMemoryClients(Config.GetClients())
                .AddTestUsers(Config.GetTestUser().ToList())
                .AddInMemoryIdentityResources(Config.GetIdentityResources())
                .AddDeveloperSigningCredential(); //clienttan gelen private key'i auth'dan gelen public key ile karþýlaþtýrýr
            services.AddSingleton(sp =>
            {
                return new RedisService(Configuration["CacheOptions:Url"]);
            });
            services.AddSingleton<IDatabase>(x =>
            {
                var redisService = x.GetRequiredService<RedisService>();
                return redisService.GetDb(0);
            });
            services.AddScoped<IRepository>(x =>
            {
                var repo = new Repository();
                var redisService = x.GetRequiredService<RedisService>();
                return new RepositoryWithCache(repo, redisService);
            });
            services.AddControllersWithViews();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseRouting();
            app.UseIdentityServer();
            app.UseStaticFiles(); //wwwroot'a eriþim için
            app.UseAuthentication(); //kimlik doðrulama için
            app.UseAuthorization(); //yetkilendirme için

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}

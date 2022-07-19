using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AuthServer
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
          
            services.AddIdentityServer()
                .AddInMemoryApiResources(Config.GetApiResources())
                .AddInMemoryApiScopes(Config.GetApiScopes())
                .AddInMemoryClients(Config.GetClients())
                .AddDeveloperSigningCredential();
            services.AddControllersWithViews();

        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseStaticFiles(); //wwwroot'a eri�im i�in
            app.UseAuthentication(); //kimlik do�rulama i�in
            app.UseAuthorization(); //yetkilendirme i�in

            app.UseIdentityServer();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
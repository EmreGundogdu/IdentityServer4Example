using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace HalkBankAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                     .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
                     {
                        //Token'ı yayınlayan Auth Server adresi bildiriliyor. Yani yetkiyi dağıtan mekanizmanın adresi bildirilerek ilgili API ile ilişkilendiriliyor.
                        options.Authority = "https://localhost:1000";
                        //Auth Server uygulamasındaki 'Garanti' isimli resource ile bu API ilişkilendiriliyor.
                        options.Audience = "HalkBank";
                     });
            services.AddAuthorization(_ =>
            {
                _.AddPolicy("ReadGaranti", policy => policy.RequireClaim("scope", "Garanti.Read"));
                _.AddPolicy("WriteGaranti", policy => policy.RequireClaim("scope", "Garanti.Write"));
                _.AddPolicy("ReadWriteGaranti", policy => policy.RequireClaim("scope", "Garanti.Write", "Garanti.Read"));
                _.AddPolicy("AllGaranti", policy => policy.RequireClaim("scope", "Garanti.Admin"));
                _.AddPolicy("ReadHalkBank", policy => policy.RequireClaim("scope", "HalkBank.Read"));
                _.AddPolicy("WriteHalkBank", policy => policy.RequireClaim("scope", "HalkBank.Write"));
                _.AddPolicy("ReadWriteHalkBank", policy => policy.RequireClaim("scope", "HalkBank.Write", "HalkBank.Read"));
                _.AddPolicy("AllHalkBank", policy => policy.RequireClaim("scope", "HalkBank.Admin"));
            });
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}

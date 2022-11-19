using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace OnlineBankamatik
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
            services.AddAuthentication(options =>
            {
                options.DefaultScheme = "OnlineBankamatikCookie";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("OnlineBankamatikCookie")
            .AddOpenIdConnect("oidc", options =>
            {
                options.SignInScheme = "OnlineBankamatikCookie";
                options.Authority = "https://localhost:1000";
                options.ClientId = "OnlineBankamatik";
                options.ClientSecret = "onlinebankamatik";
                options.ResponseType = "code id_token";
                options.GetClaimsFromUserInfoEndpoint = false;
                options.SaveTokens = true; //Auth server’dan ilgili access token deðeri client tarafýndan elde edilmiþ olacaktýr.
                options.Scope.Add("offline_access");
                options.Scope.Add("Garanti.Write");
                options.Scope.Add("Garanti.Read");
            });
            services.AddHttpClient();
            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Bankamatik}/{action=Index}/{id?}");
            });
        }
    }
}

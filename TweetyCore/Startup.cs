using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using System;
using TweetyCore.EntityFramework;
using TweetyCore.Utils;
using TweetyCore.Utils.StringMatcher;
using TweetyCore.Utils.Twitter;

namespace TweetyCore
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddDbContext<TweetyDbContext>(options => options.UseSqlite("tweety.db"));

            // register the scope authorization handler
            services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
            services.AddScoped<IKMP, KMP>();
            services.AddScoped<IBooyer, Booyer>();
            services.AddScoped<ITwitterConnect, TwitterConnect>();

            services.AddCors();

            services.AddSwaggerGen();

            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
               .AddMicrosoftIdentityWebApp(Configuration);

            services.AddAuthentication().AddMicrosoftIdentityWebApi(Configuration);

            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddMicrosoftIdentityUI();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json",
                                 "WebApp1 v1"));
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCookiePolicy();
            var corsAllow = Environment.GetEnvironmentVariable("CORS_ALLOW") ?? "*";
            app.UseCors(builder => builder.WithOrigins(corsAllow).AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}

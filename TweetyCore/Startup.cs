using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using Tweetinvi;
using Tweetinvi.Models;
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
            string domain = $"https://{Configuration["Auth0:Domain"]}/";
            services
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = domain;
                    options.Audience = Environment.GetEnvironmentVariable("AUTH0_AUDIANCE");
                });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("read:users", policy => policy.Requirements.Add(new HasScopeRequirement("read:users", domain)));
            });

            _Connect();

            services.AddControllersWithViews();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCookiePolicy();
            app.UseCors(builder => builder.WithOrigins(Environment.GetEnvironmentVariable("CORS_ALLOW")).AllowAnyHeader());
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }

        private void _Connect()
        {
            string customer_key = Environment.GetEnvironmentVariable("CUSTOMER_KEY");
            string customer_secret = Environment.GetEnvironmentVariable("CUSTOMER_SECRET");
            string token = Environment.GetEnvironmentVariable("TOKEN");
            string token_secret = Environment.GetEnvironmentVariable("TOKEN_SECRET");
            // When a new thread is created, the default credentials will be the Application Credentials
            Auth.ApplicationCredentials = new TwitterCredentials(customer_key, customer_secret, token, token_secret);
        }
    }
}

using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using NLog.Web;
using Tweetinvi;
using TweetyCore.ConfigModel;
using TweetyCore.EntityFramework;
using TweetyCore.Utils.StringMatcher;
using TweetyCore.Utils.Twitter;

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureLogging(logging =>
{
    logging.ClearProviders();
    logging.SetMinimumLevel(LogLevel.Trace);
    logging.AddNLog("Nlog.config");
    logging.AddNLogWeb();
});
builder.Host.UseNLog();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddDbContext<TweetyDbContext>(options => options.UseSqlite("tweety.db"));

var twitterConfig = new TwitterConfig();
builder.Configuration.GetSection("Twitter").Bind(twitterConfig);
builder.Services.AddSingleton(twitterConfig);
// register the scope authorization handler
builder.Services.AddScoped<IKMP, KMP>();
builder.Services.AddScoped<IBooyer, Booyer>();
builder.Services.AddScoped<ITwitterClient, TwitterConsumer>();
builder.Services.AddScoped<ITwitterConnect, TwitterConnect>();

builder.Services.AddCors();

builder.Services.AddSwaggerGen();

builder.Services.AddMicrosoftIdentityWebAppAuthentication(builder.Configuration, "AzureAD")
    .EnableTokenAcquisitionToCallDownstreamApi()
      .AddInMemoryTokenCaches();

builder.Services.AddAuthentication()
    .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAD"), JwtBearerDefaults.AuthenticationScheme)
    .EnableTokenAcquisitionToCallDownstreamApi();

builder.Services.AddControllersWithViews(options =>
{
    var policy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
    options.Filters.Add(new AuthorizeFilter(policy));
}).AddMicrosoftIdentityUI();

builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders =
        ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

var app = builder.Build();

app.UseForwardedHeaders();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json",
                     "WebApp1 v1"));
}
else
{
    app.UseHsts();
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

app.Run();

public partial class Program { }
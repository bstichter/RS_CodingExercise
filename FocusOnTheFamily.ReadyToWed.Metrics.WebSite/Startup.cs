using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore;

using AspNet.Security.OAuth.GitHub;

using FocusOnTheFamily.ReadyToWed.Metrics.DataModel;

namespace FocusOnTheFamily.ReadyToWed.Metrics.WebSite {
  public class Startup {
    public Startup(IHostingEnvironment env) {
      var builder = new ConfigurationBuilder()
          .SetBasePath(env.ContentRootPath)
          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
          .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

      if (env.IsDevelopment()) {
        // For more details on using the user secret store see https://go.microsoft.com/fwlink/?LinkID=532709
        builder.AddUserSecrets<Startup>();
      }

      builder.AddEnvironmentVariables();
      Configuration = builder.Build();
    }

    public IConfigurationRoot Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services) {
      //Register the metrics database
      services.AddDbContext<ReadyToWedMetricsContext>(
        options => options.UseInMemoryDatabase()
      );

      //Register Cookie Authentication
      services.AddAuthentication(
        options => {
          options.SignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        }
      );

      //Register MVC
      services.AddMvc(
        config => {
          config.Filters.Add(
            new AuthorizeFilter(
              new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build()
            )
          );
        }
      );
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory) {
      loggerFactory.AddConsole(Configuration.GetSection("Logging"));
      loggerFactory.AddDebug();

      if (env.IsDevelopment()) {
        app.UseDeveloperExceptionPage();
        app.UseBrowserLink();
      } else {
        app.UseExceptionHandler("/Home/Error");
      }

      app.UseStaticFiles();

      //Configure Authentication
      app.UseCookieAuthentication(
        new CookieAuthenticationOptions {
          AutomaticAuthenticate = true,
          AutomaticChallenge = true,
          LoginPath = new PathString("/Home/Login"),
          ReturnUrlParameter = "returnUrl",
          LogoutPath = new PathString("/Home/Logout")
        }
      );

      app.UseGitHubAuthentication(
        new GitHubAuthenticationOptions {
          ClientId = Configuration["GitHub:ClientId"],
          ClientSecret = Configuration["GitHub:ClientSecret"],
          Scope = { "user:email" }
        }
      );

      app.UseMvc(
        routes => {
          routes.MapRoute(
            name: "default",
            template: "{controller=Home}/{action=Index}/{id?}"
          );
        }
      );

      //Load the metrics database
      app.LoadData(
        new DataLoaderOptions {
          UsersFile = Path.Combine(env.WebRootPath, Configuration["ReadyToWedMetrics:UsersFile"]),
          DailyNumbersFile =
              Path.Combine(env.WebRootPath, Configuration["ReadyToWedMetrics:DailyNumbersFile"])
        }
      );
    }
  }
}

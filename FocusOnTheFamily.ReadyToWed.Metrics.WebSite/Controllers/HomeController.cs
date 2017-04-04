using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

using FocusOnTheFamily.ReadyToWed.Metrics.DataModel;
using FocusOnTheFamily.ReadyToWed.Metrics.BusinessModel;
using static FocusOnTheFamily.ReadyToWed.Metrics.BusinessModel.MarketingDomainBusinessHandler;
using FocusOnTheFamily.ReadyToWed.Metrics.WebSite.MarketingViewModels;

namespace FocusOnTheFamily.ReadyToWed.Metrics.WebSite {
  public class HomeController : Controller {
    private readonly ReadyToWedMetricsContext Context;

    public HomeController(ReadyToWedMetricsContext context) {
      this.Context = context;
    }

    [AllowAnonymous]
    public IActionResult Index() {
      return View();
    }

    public IActionResult About() {
      return View();
    }

    public IActionResult Marketing() {
      var businessHandler = new MarketingDomainBusinessHandler();
      businessHandler.Init(this.Context);

      UserMetrics userMetrics = businessHandler.GetUserMetrics();
      DailyMetrics dailyMetrics = businessHandler.GetDailyMetrics();

      int numberOfMaleUsers = (
        from x in userMetrics.NumberOfUsersByGender
        where x.Gender == Gender.Male
        select x.Count
      ).SingleOrDefault();

      int numberOfUsers = (
        from x in userMetrics.NumberOfUsersByGender
        select x.Count
      ).Sum();


      //Only calculating only one percentage and subtracting to be sure the percentages totals 100%.
      int percentOfMaleUsers = (int) Math.Round(100.0M * numberOfMaleUsers / numberOfUsers, 0);

      //Admittedly, the action and view are Marketing, but the view model is 
      //Marketing Metrics... this is because I'm assuming Marketing will get
      //pages some day instead of just this one. Also, using decimals to be sure
      //they will display precisely to the rounded figures.
      var viewModel = new MetricsViewModel() {
        AverageAgeOfUsers = Math.Round((decimal) userMetrics.AverageAgeOfUsers,1),
        PercentOfUsersMale = percentOfMaleUsers,
        PercentOfUsersFemale = 100 - percentOfMaleUsers,
        AverageInstallsPerDay = Math.Round((decimal)dailyMetrics.AverageInstalls,1),
        AverageLoginsPerDay = Math.Round((decimal)dailyMetrics.AverageLogins,1)
      };

      return View(viewModel);
    }

    public IActionResult Error() {
      return View();
    }

    [AllowAnonymous]
    async public Task Login(string returnUrl) {
      //Verify returnUrl exists and is part of this website
      if (!Url.IsLocalUrl(returnUrl) || String.IsNullOrEmpty(returnUrl)) {
        returnUrl = Url.Action(String.Empty); //Go to the default action.
      }

      // Return a challenge to invoke the GigHub authentication scheme
      await this.HttpContext.Authentication.ChallengeAsync("GitHub", properties: new AuthenticationProperties() { RedirectUri = returnUrl });
    }

    [AllowAnonymous]
    async public Task<IActionResult> Logout() {
      // Sign the user out of the authentication middleware (i.e. it will clear the Auth cookie)
      await this.HttpContext.Authentication.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

      // Redirect the user to the home page after signing out
      return Redirect(Url.Action(String.Empty));
    }
  }
}

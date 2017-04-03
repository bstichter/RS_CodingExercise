using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace FocusOnTheFamily.ReadyToWed.Metrics.WebSite {
  public class HomeController : Controller {
    [AllowAnonymous]
    public IActionResult Index() {
      return View();
    }

    public IActionResult About() {
      ViewData["Message"] = "Your application description page.";

      return View();
    }

    public IActionResult Contact() {
      ViewData["Message"] = "Your contact page.";

      return View();
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

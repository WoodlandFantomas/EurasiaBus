using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace EurasiaBus.Controllers
{
    public class LanguageController : Controller
    {
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetLanguage(string culture, string returnUrl = "/")
        {
            var supportedCultures = new[] { "ru", "uk", "en" };

            if (!supportedCultures.Contains(culture))
            {
                culture = "ru";
            }

            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(
                    new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true
                });

            if (!Url.IsLocalUrl(returnUrl))
            {
                returnUrl = "/";
            }

            return LocalRedirect(returnUrl);
        }
    }
}
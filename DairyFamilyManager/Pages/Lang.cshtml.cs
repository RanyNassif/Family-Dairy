using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DairyFamilyManager.Pages;

public class LangModel : PageModel
{
    public IActionResult OnGet(string culture, string? returnUrl)
    {
        if (string.IsNullOrWhiteSpace(culture))
        {
            culture = "en-US";
        }

        Response.Cookies.Append(CookieRequestCultureProvider.DefaultCookieName, CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)), new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) });

        if (string.IsNullOrWhiteSpace(returnUrl))
        {
            returnUrl = "/";
        }

        return LocalRedirect(returnUrl);
    }
}

using DairyFamilyManager.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace DairyFamilyManager.Pages;
[AllowAnonymous]
public class LoginModel : PageModel
{
    private readonly AuthService _auth;

    public LoginModel(AuthService auth)
    {
        _auth = auth;
    }

    [BindProperty] public string Username { get; set; } = "";
    [BindProperty] public string Password { get; set; } = "";
    [BindProperty(SupportsGet = true)] public string ReturnUrl { get; set; } = "/";

    public string Error { get; set; } = "";

    public IActionResult OnGet()
    {
        if (User.Identity != null && User.Identity.IsAuthenticated)
        {
            return Redirect("/Home");

        }

        if (string.IsNullOrWhiteSpace(ReturnUrl))
        {
            ReturnUrl = "/";
        }

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        var user = await _auth.ValidateAsync(Username, Password);
        if (user == null)
        {
            Error = "Invalid username or password.";
            return Page();
        }

        List<Claim> claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role)
    };

        ClaimsIdentity identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        ClaimsPrincipal principal = new ClaimsPrincipal(identity);

        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
        {
            IsPersistent = true,
            AllowRefresh = true
        });

        return Redirect("/Home");
    }

}

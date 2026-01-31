using DairyFamilyManager.Data;
using DairyFamilyManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DairyFamilyManager.Pages.Clients;

[Authorize(Policy = "AdminOnly")]
public class CreateModel : PageModel
{
    private readonly AppDbContext _db;

    public CreateModel(AppDbContext db)
    {
        _db = db;
    }

    [BindProperty] public string NameEn { get; set; } = "";
    [BindProperty] public string NameAr { get; set; } = "";
    [BindProperty] public bool UsesDistributor { get; set; }
    [BindProperty] public bool IsActive { get; set; } = true;

    public string Error { get; set; } = "";

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(NameEn) || string.IsNullOrWhiteSpace(NameAr))
        {
            Error = "Name EN and Name AR are required.";
            return Page();
        }

        Client c = new Client
        {
            NameEn = NameEn.Trim(),
            NameAr = NameAr.Trim(),
            UsesDistributor = UsesDistributor,
            IsActive = IsActive
        };

        _db.Clients.Add(c);
        await _db.SaveChangesAsync();

        return Redirect("/Clients");
    }
}

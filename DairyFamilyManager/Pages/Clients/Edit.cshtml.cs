using DairyFamilyManager.Data;
using DairyFamilyManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DairyFamilyManager.Pages.Clients;

[Authorize(Policy = "AdminOnly")]
public class EditModel : PageModel
{
    private readonly AppDbContext _db;

    public EditModel(AppDbContext db)
    {
        _db = db;
    }

    [BindProperty] public long Id { get; set; }
    [BindProperty] public string NameEn { get; set; } = "";
    [BindProperty] public string NameAr { get; set; } = "";
    [BindProperty] public bool UsesDistributor { get; set; }
    [BindProperty] public bool IsActive { get; set; } = true;

    public string Error { get; set; } = "";

    public async Task<IActionResult> OnGetAsync(long id)
    {
        Client? c = await _db.Clients.FindAsync(id);
        if (c == null) return Redirect("/Clients");

        Id = c.Id;
        NameEn = c.NameEn;
        NameAr = c.NameAr;
        UsesDistributor = c.UsesDistributor;
        IsActive = c.IsActive;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(NameEn) || string.IsNullOrWhiteSpace(NameAr))
        {
            Error = "Name EN and Name AR are required.";
            return Page();
        }

        Client? c = await _db.Clients.FindAsync(Id);
        if (c == null) return Redirect("/Clients");

        c.NameEn = NameEn.Trim();
        c.NameAr = NameAr.Trim();
        c.UsesDistributor = UsesDistributor;
        c.IsActive = IsActive;

        await _db.SaveChangesAsync();

        return Redirect("/Clients");
    }
}

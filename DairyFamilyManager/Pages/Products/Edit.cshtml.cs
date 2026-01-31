using DairyFamilyManager.Data;
using DairyFamilyManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DairyFamilyManager.Pages.Products;

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
    [BindProperty] public string? LabelEn { get; set; }
    [BindProperty] public string? LabelAr { get; set; }

    [BindProperty] public decimal BasePrice { get; set; }

    [BindProperty] public ProfitType FactoryProfitType { get; set; }
    [BindProperty] public decimal FactoryProfitValue { get; set; }

    [BindProperty] public ProfitType DistributorProfitType { get; set; }
    [BindProperty] public decimal DistributorProfitValue { get; set; }

    [BindProperty] public bool IsActive { get; set; } = true;

    public string Error { get; set; } = "";

    public async Task<IActionResult> OnGetAsync(long id)
    {
        Product? p = await _db.Products.FindAsync(id);
        if (p == null) return Redirect("/Products");

        Id = p.Id;
        NameEn = p.NameEn;
        NameAr = p.NameAr;
        LabelEn = p.LabelEn;
        LabelAr = p.LabelAr;
        BasePrice = p.BasePrice;
        FactoryProfitType = p.FactoryProfitType;
        FactoryProfitValue = p.FactoryProfitValue;
        DistributorProfitType = p.DistributorProfitType;
        DistributorProfitValue = p.DistributorProfitValue;
        IsActive = p.IsActive;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (string.IsNullOrWhiteSpace(NameEn) || string.IsNullOrWhiteSpace(NameAr))
        {
            Error = "Name EN and Name AR are required.";
            return Page();
        }

        if (BasePrice < 0)
        {
            Error = "Base price must be >= 0.";
            return Page();
        }

        if (FactoryProfitValue < 0 || DistributorProfitValue < 0)
        {
            Error = "Profit values must be >= 0.";
            return Page();
        }

        Product? p = await _db.Products.FindAsync(Id);
        if (p == null) return Redirect("/Products");

        p.NameEn = NameEn.Trim();
        p.NameAr = NameAr.Trim();
        p.LabelEn = string.IsNullOrWhiteSpace(LabelEn) ? null : LabelEn.Trim();
        p.LabelAr = string.IsNullOrWhiteSpace(LabelAr) ? null : LabelAr.Trim();
        p.BasePrice = BasePrice;
        p.FactoryProfitType = FactoryProfitType;
        p.FactoryProfitValue = FactoryProfitValue;
        p.DistributorProfitType = DistributorProfitType;
        p.DistributorProfitValue = DistributorProfitValue;
        p.IsActive = IsActive;

        await _db.SaveChangesAsync();

        return Redirect("/Products");
    }
}

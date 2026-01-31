using DairyFamilyManager.Data;
using DairyFamilyManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DairyFamilyManager.Pages.Products;

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
    [BindProperty] public string? LabelEn { get; set; }
    [BindProperty] public string? LabelAr { get; set; }

    [BindProperty] public decimal BasePrice { get; set; }

    [BindProperty] public ProfitType FactoryProfitType { get; set; } = ProfitType.Fixed;
    [BindProperty] public decimal FactoryProfitValue { get; set; }

    [BindProperty] public ProfitType DistributorProfitType { get; set; } = ProfitType.Fixed;
    [BindProperty] public decimal DistributorProfitValue { get; set; }

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

        Product p = new Product
        {
            NameEn = NameEn.Trim(),
            NameAr = NameAr.Trim(),
            LabelEn = string.IsNullOrWhiteSpace(LabelEn) ? null : LabelEn.Trim(),
            LabelAr = string.IsNullOrWhiteSpace(LabelAr) ? null : LabelAr.Trim(),
            BasePrice = BasePrice,
            FactoryProfitType = FactoryProfitType,
            FactoryProfitValue = FactoryProfitValue,
            DistributorProfitType = DistributorProfitType,
            DistributorProfitValue = DistributorProfitValue,
            IsActive = IsActive
        };

        _db.Products.Add(p);
        await _db.SaveChangesAsync();

        return Redirect("/Products");
    }
}

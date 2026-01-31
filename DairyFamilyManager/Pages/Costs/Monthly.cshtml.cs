using DairyFamilyManager.Data;
using DairyFamilyManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DairyFamilyManager.Pages.Costs;

[Authorize(Policy = "AdminOnly")]
public class MonthlyModel : PageModel
{
    private readonly AppDbContext _db;

    public MonthlyModel(AppDbContext db)
    {
        _db = db;
    }

    [BindProperty(SupportsGet = true)] public int Year { get; set; } = DateTime.Today.Year;
    [BindProperty(SupportsGet = true)] public int Month { get; set; } = DateTime.Today.Month;

    [BindProperty] public List<RowVm> Rows { get; set; } = new List<RowVm>();

    public string Error { get; set; } = "";
    public string Message { get; set; } = "";

    public class RowVm
    {
        public long ProductId { get; set; }
        public string ProductNameEn { get; set; } = "";
        public string ProductNameAr { get; set; } = "";

        public string? MilkCostText { get; set; }
        public string? WorkersCostText { get; set; }
        public string? GasCostText { get; set; }
        public string? OtherCostText { get; set; }
    }

    public async Task OnGetAsync()
    {
        await LoadAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (Year < 2000 || Year > 2100)
        {
            Error = "Invalid year.";
            await LoadAsync();
            return Page();
        }

        if (Month < 1 || Month > 12)
        {
            Error = "Invalid month.";
            await LoadAsync();
            return Page();
        }

        List<long> productIds = Rows.Select(x => x.ProductId).Distinct().ToList();

        List<MonthlyProductCost> existing = await _db.MonthlyProductCosts
            .Where(x => x.Year == Year && x.Month == Month && productIds.Contains(x.ProductId))
            .ToListAsync();

        for (int i = 0; i < Rows.Count; i++)
        {
            long productId = Rows[i].ProductId;

            decimal milk = ParseMoney(Rows[i].MilkCostText, out bool ok1);
            decimal workers = ParseMoney(Rows[i].WorkersCostText, out bool ok2);
            decimal gas = ParseMoney(Rows[i].GasCostText, out bool ok3);
            decimal other = ParseMoney(Rows[i].OtherCostText, out bool ok4);

            if (!ok1 || !ok2 || !ok3 || !ok4)
            {
                Error = "Invalid cost value. Use numbers like 10 or 10.5 or 10.125";
                await LoadAsync();
                return Page();
            }

            MonthlyProductCost? row = existing.FirstOrDefault(x => x.ProductId == productId);
            if (row == null)
            {
                row = new MonthlyProductCost
                {
                    Year = Year,
                    Month = Month,
                    ProductId = productId,
                    MilkCost = milk,
                    WorkersCost = workers,
                    GasCost = gas,
                    OtherCost = other
                };
                _db.MonthlyProductCosts.Add(row);
            }
            else
            {
                row.MilkCost = milk;
                row.WorkersCost = workers;
                row.GasCost = gas;
                row.OtherCost = other;
            }
        }

        await _db.SaveChangesAsync();

        Message = "Saved.";
        return Redirect("/Costs/Monthly?year=" + Year + "&month=" + Month);
    }

    private async Task LoadAsync()
    {
        if (Year < 2000 || Year > 2100) Year = DateTime.Today.Year;
        if (Month < 1 || Month > 12) Month = DateTime.Today.Month;

        List<Product> products = await _db.Products.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.NameEn).ToListAsync();

        List<long> productIds = products.Select(x => x.Id).ToList();

        List<MonthlyProductCost> costs = await _db.MonthlyProductCosts
            .AsNoTracking()
            .Where(x => x.Year == Year && x.Month == Month && productIds.Contains(x.ProductId))
            .ToListAsync();

        Rows = products.Select(p =>
        {
            MonthlyProductCost? c = costs.FirstOrDefault(x => x.ProductId == p.Id);
            return new RowVm
            {
                ProductId = p.Id,
                ProductNameEn = p.NameEn,
                ProductNameAr = p.NameAr,
                MilkCostText = (c == null ? 0m : c.MilkCost).ToString("0.###", CultureInfo.InvariantCulture),
                WorkersCostText = (c == null ? 0m : c.WorkersCost).ToString("0.###", CultureInfo.InvariantCulture),
                GasCostText = (c == null ? 0m : c.GasCost).ToString("0.###", CultureInfo.InvariantCulture),
                OtherCostText = (c == null ? 0m : c.OtherCost).ToString("0.###", CultureInfo.InvariantCulture)
            };
        }).ToList();
    }

    private decimal ParseMoney(string? txt, out bool ok)
    {
        string v = txt == null ? "" : txt.Trim();
        if (string.IsNullOrWhiteSpace(v))
        {
            ok = true;
            return 0m;
        }

        ok = decimal.TryParse(v, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d);
        if (!ok) return 0m;
        if (d < 0m)
        {
            ok = false;
            return 0m;
        }

        return Math.Round(d, 3);
    }
}

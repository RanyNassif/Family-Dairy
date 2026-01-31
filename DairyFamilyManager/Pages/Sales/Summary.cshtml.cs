using DairyFamilyManager.Data;
using DairyFamilyManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DairyFamilyManager.Pages.Sales;

[Authorize(Policy = "DataEntryOrAdmin")]
public class SummaryModel : PageModel
{
    private readonly AppDbContext _db;

    public SummaryModel(AppDbContext db)
    {
        _db = db;
    }

    [BindProperty(SupportsGet = true)] public string DateString { get; set; } = DateTime.Today.ToString("yyyy-MM-dd");
    [BindProperty] public string? BenzineText { get; set; }

    public decimal TotalSales { get; set; }
    public decimal TotalFactoryProfit { get; set; }
    public decimal TotalDistributorProfit { get; set; }
    public decimal DistributorNet { get; set; }
    public List<ClientSummaryRow> ByClient { get; set; } = new List<ClientSummaryRow>();
    public List<ProductSummaryRow> ByProduct { get; set; } = new List<ProductSummaryRow>();

    public string Error { get; set; } = "";

    public async Task OnGetAsync()
    {
        await LoadAsync();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        bool ok = DateTime.TryParseExact(DateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
        if (!ok)
        {
            Error = "Invalid date.";
            await LoadAsync();
            return Page();
        }

        Distributor? dist = await _db.Distributors.AsNoTracking().FirstOrDefaultAsync(x => x.IsActive);
        if (dist == null)
        {
            Error = "No distributor found.";
            await LoadAsync();
            return Page();
        }

        string txt = BenzineText == null ? "" : BenzineText.Trim();
        bool parsed = decimal.TryParse(txt, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal benzine);
        if (!parsed || benzine < 0)
        {
            Error = "Invalid benzine amount.";
            await LoadAsync();
            return Page();
        }

        DistributorDailyExpense? existing = await _db.DistributorDailyExpenses.FirstOrDefaultAsync(x => x.Date == date.Date && x.DistributorId == dist.Id);
        if (existing == null)
        {
            _db.DistributorDailyExpenses.Add(new DistributorDailyExpense
            {
                Date = date.Date,
                DistributorId = dist.Id,
                BenzineAmount = benzine
            });
        }
        else
        {
            existing.BenzineAmount = benzine;
        }

        await _db.SaveChangesAsync();

        return Redirect("/Sales/Summary?date=" + DateString);
    }

    private async Task LoadAsync()
    {
        ByClient = new List<ClientSummaryRow>();
        ByProduct = new List<ProductSummaryRow>();

        bool ok = DateTime.TryParseExact(DateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
        if (!ok)
        {
            Error = "Invalid date.";
            return;
        }

        List<DailySale> sales = await _db.DailySales
            .Include(x => x.Client)
            .Include(x => x.Lines)
            .ThenInclude(l => l.Product)
            .AsNoTracking()
            .Where(x => x.Date == date.Date)
            .ToListAsync();

        Dictionary<long, ClientSummaryRow> byClient = new Dictionary<long, ClientSummaryRow>();
        Dictionary<long, ProductSummaryRow> byProduct = new Dictionary<long, ProductSummaryRow>();

        decimal totalSales = 0m;
        decimal totalFactory = 0m;
        decimal totalDist = 0m;

        for (int s = 0; s < sales.Count; s++)
        {
            DailySale sale = sales[s];

            long clientId = sale.ClientId;
            string clientEn = sale.Client == null ? "" : sale.Client.NameEn;
            string clientAr = sale.Client == null ? "" : sale.Client.NameAr;
            bool usesDistributor = sale.Client != null && sale.Client.UsesDistributor;

            if (!byClient.TryGetValue(clientId, out ClientSummaryRow? cRow))
            {
                cRow = new ClientSummaryRow
                {
                    ClientId = clientId,
                    ClientNameEn = clientEn,
                    ClientNameAr = clientAr,
                    UsesDistributor = usesDistributor,
                    SalesTotal = 0m,
                    FactoryProfitTotal = 0m,
                    DistributorProfitTotal = 0m
                };
                byClient[clientId] = cRow;
            }

            for (int i = 0; i < sale.Lines.Count; i++)
            {
                DailySaleLine l = sale.Lines[i];

                decimal lineTotal = l.UnitPriceUsed * l.Quantity;

                decimal factoryPerUnit = l.FactoryProfitTypeUsed == ProfitType.Percent ? l.UnitPriceUsed * l.FactoryProfitValueUsed / 100m : l.FactoryProfitValueUsed;
                decimal factoryLine = factoryPerUnit * l.Quantity;

                decimal distLine = 0m;
                if (usesDistributor)
                {
                    decimal distPerUnit = l.DistributorProfitTypeUsed == ProfitType.Percent ? l.UnitPriceUsed * l.DistributorProfitValueUsed / 100m : l.DistributorProfitValueUsed;
                    distLine = distPerUnit * l.Quantity;
                }

                totalSales += lineTotal;
                totalFactory += factoryLine;
                totalDist += distLine;

                cRow.SalesTotal += lineTotal;
                cRow.FactoryProfitTotal += factoryLine;
                cRow.DistributorProfitTotal += distLine;

                long productId = l.ProductId;
                string prodEn = l.Product == null ? "" : l.Product.NameEn;
                string prodAr = l.Product == null ? "" : l.Product.NameAr;

                if (!byProduct.TryGetValue(productId, out ProductSummaryRow? pRow))
                {
                    pRow = new ProductSummaryRow
                    {
                        ProductId = productId,
                        ProductNameEn = prodEn,
                        ProductNameAr = prodAr,
                        QuantityTotal = 0m,
                        SalesTotal = 0m,
                        FactoryProfitTotal = 0m,
                        DistributorProfitTotal = 0m
                    };
                    byProduct[productId] = pRow;
                }

                pRow.QuantityTotal += l.Quantity;
                pRow.SalesTotal += lineTotal;
                pRow.FactoryProfitTotal += factoryLine;
                pRow.DistributorProfitTotal += distLine;
            }
        }

        TotalSales = Math.Round(totalSales, 3);
        TotalFactoryProfit = Math.Round(totalFactory, 3);
        TotalDistributorProfit = Math.Round(totalDist, 3);

        ByClient = byClient.Values
            .Select(x =>
            {
                x.SalesTotal = Math.Round(x.SalesTotal, 3);
                x.FactoryProfitTotal = Math.Round(x.FactoryProfitTotal, 3);
                x.DistributorProfitTotal = Math.Round(x.DistributorProfitTotal, 3);
                return x;
            })
            .OrderByDescending(x => x.SalesTotal)
            .ToList();

        ByProduct = byProduct.Values
            .Select(x =>
            {
                x.QuantityTotal = Math.Round(x.QuantityTotal, 3);
                x.SalesTotal = Math.Round(x.SalesTotal, 3);
                x.FactoryProfitTotal = Math.Round(x.FactoryProfitTotal, 3);
                x.DistributorProfitTotal = Math.Round(x.DistributorProfitTotal, 3);
                return x;
            })
            .OrderByDescending(x => x.QuantityTotal)
            .ToList();

        Distributor? distObj = await _db.Distributors.AsNoTracking().FirstOrDefaultAsync(x => x.IsActive);
        decimal benzine = 0m;

        if (distObj != null)
        {
            DistributorDailyExpense? exp = await _db.DistributorDailyExpenses.AsNoTracking().FirstOrDefaultAsync(x => x.Date == date.Date && x.DistributorId == distObj.Id);
            if (exp != null) benzine = exp.BenzineAmount;
        }

        BenzineText = benzine.ToString("0.###", CultureInfo.InvariantCulture);
        DistributorNet = Math.Round(TotalDistributorProfit - benzine, 3);
    }

    public class ClientSummaryRow
    {
        public long ClientId { get; set; }
        public string ClientNameEn { get; set; } = "";
        public string ClientNameAr { get; set; } = "";
        public bool UsesDistributor { get; set; }

        public decimal SalesTotal { get; set; }
        public decimal FactoryProfitTotal { get; set; }
        public decimal DistributorProfitTotal { get; set; }
    }

    public class ProductSummaryRow
    {
        public long ProductId { get; set; }
        public string ProductNameEn { get; set; } = "";
        public string ProductNameAr { get; set; } = "";

        public decimal QuantityTotal { get; set; }
        public decimal SalesTotal { get; set; }
        public decimal FactoryProfitTotal { get; set; }
        public decimal DistributorProfitTotal { get; set; }
    }

}

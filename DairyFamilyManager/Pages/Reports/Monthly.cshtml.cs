using DairyFamilyManager.Data;
using DairyFamilyManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace DairyFamilyManager.Pages.Reports;

[Authorize(Policy = "AdminOnly")]
public class MonthlyModel : PageModel
{
    private readonly AppDbContext _db;

    public MonthlyModel(AppDbContext db)
    {
        _db = db;
    }

    public int Year { get; set; } = DateTime.Today.Year;
    public int Month { get; set; } = DateTime.Today.Month;

    public decimal TotalSales { get; set; }
    public decimal FactoryProfit { get; set; }
    public decimal DistributorProfit { get; set; }
    public decimal Benzine { get; set; }
    public decimal DistributorNet { get; set; }
    public decimal TotalCosts { get; set; }
    public decimal FactoryNetAfterCosts { get; set; }

    public string DailyLabelsJson { get; set; } = "[]";
    public string DailySalesJson { get; set; } = "[]";
    public string DailyFactoryProfitJson { get; set; } = "[]";
    public string DailyDistributorNetJson { get; set; } = "[]";

    public string TopProductLabelsJson { get; set; } = "[]";
    public string TopProductSalesJson { get; set; } = "[]";

    public string Error { get; set; } = "";

    public class ProductRow
    {
        public long ProductId { get; set; }
        public string ProductNameEn { get; set; } = "";
        public string ProductNameAr { get; set; } = "";
        public decimal QuantityTotal { get; set; }
        public decimal SalesTotal { get; set; }
        public decimal FactoryProfitTotal { get; set; }
        public decimal DistributorProfitTotal { get; set; }
    }

    public List<ProductRow> TopBySales { get; set; } = new List<ProductRow>();

    public async Task OnGetAsync(int? year, int? month)
    {
        if (year.HasValue) Year = year.Value;
        if (month.HasValue) Month = month.Value;

        if (Year < 2000 || Year > 2100)
        {
            Error = "Invalid year.";
            return;
        }

        if (Month < 1 || Month > 12)
        {
            Error = "Invalid month.";
            return;
        }

        DateTime start = new DateTime(Year, Month, 1);
        DateTime end = start.AddMonths(1);

        List<DailySale> sales = await _db.DailySales
            .Include(x => x.Client)
            .Include(x => x.Lines)
            .ThenInclude(l => l.Product)
            .AsNoTracking()
            .Where(x => x.Date >= start && x.Date < end)
            .ToListAsync();

        Dictionary<long, ProductRow> byProduct = new Dictionary<long, ProductRow>();

        decimal totalSales = 0m;
        decimal totalFactory = 0m;
        decimal totalDist = 0m;

        for (int s = 0; s < sales.Count; s++)
        {
            DailySale sale = sales[s];
            bool usesDistributor = sale.Client != null && sale.Client.UsesDistributor;

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

                long productId = l.ProductId;

                if (!byProduct.TryGetValue(productId, out ProductRow? pRow))
                {
                    pRow = new ProductRow
                    {
                        ProductId = productId,
                        ProductNameEn = l.Product == null ? "" : l.Product.NameEn,
                        ProductNameAr = l.Product == null ? "" : l.Product.NameAr,
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
        FactoryProfit = Math.Round(totalFactory, 3);
        DistributorProfit = Math.Round(totalDist, 3);

        Distributor? distObj = await _db.Distributors.AsNoTracking().FirstOrDefaultAsync(x => x.IsActive);
        decimal benzine = 0m;

        if (distObj != null)
        {
            List<DistributorDailyExpense> exp = await _db.DistributorDailyExpenses
                .AsNoTracking()
                .Where(x => x.DistributorId == distObj.Id && x.Date >= start && x.Date < end)
                .ToListAsync();

            benzine = exp.Sum(x => x.BenzineAmount);
        }

        Benzine = Math.Round(benzine, 3);
        DistributorNet = Math.Round(DistributorProfit - Benzine, 3);

        List<MonthlyProductCost> costs = await _db.MonthlyProductCosts
            .AsNoTracking()
            .Where(x => x.Year == Year && x.Month == Month)
            .ToListAsync();

        decimal totalCosts = 0m;
        for (int i = 0; i < costs.Count; i++)
        {
            totalCosts += costs[i].MilkCost + costs[i].WorkersCost + costs[i].GasCost + costs[i].OtherCost;
        }

        TotalCosts = Math.Round(totalCosts, 3);
        FactoryNetAfterCosts = Math.Round(FactoryProfit - TotalCosts, 3);

        TopBySales = byProduct.Values
            .Select(x =>
            {
                x.QuantityTotal = Math.Round(x.QuantityTotal, 3);
                x.SalesTotal = Math.Round(x.SalesTotal, 3);
                x.FactoryProfitTotal = Math.Round(x.FactoryProfitTotal, 3);
                x.DistributorProfitTotal = Math.Round(x.DistributorProfitTotal, 3);
                return x;
            })
            .OrderByDescending(x => x.SalesTotal)
            .Take(15)
            .ToList();

        Dictionary<DateTime, decimal> salesByDay = new Dictionary<DateTime, decimal>();
        Dictionary<DateTime, decimal> factoryByDay = new Dictionary<DateTime, decimal>();
        Dictionary<DateTime, decimal> distProfitByDay = new Dictionary<DateTime, decimal>();

        for (int s = 0; s < sales.Count; s++)
        {
            DailySale sale = sales[s];
            bool usesDistributor = sale.Client != null && sale.Client.UsesDistributor;

            DateTime day = sale.Date.Date;

            if (!salesByDay.ContainsKey(day))
            {
                salesByDay[day] = 0m;
                factoryByDay[day] = 0m;
                distProfitByDay[day] = 0m;
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

                salesByDay[day] += lineTotal;
                factoryByDay[day] += factoryLine;
                distProfitByDay[day] += distLine;
            }
        }

        Dictionary<DateTime, decimal> benzineByDay = new Dictionary<DateTime, decimal>();

        if (distObj != null)
        {
            List<DistributorDailyExpense> exp = await _db.DistributorDailyExpenses
                .AsNoTracking()
                .Where(x => x.DistributorId == distObj.Id && x.Date >= start && x.Date < end)
                .ToListAsync();

            for (int i = 0; i < exp.Count; i++)
            {
                benzineByDay[exp[i].Date.Date] = exp[i].BenzineAmount;
            }
        }

        List<string> labels = new List<string>();
        List<decimal> dailySales = new List<decimal>();
        List<decimal> dailyFactory = new List<decimal>();
        List<decimal> dailyDistNet = new List<decimal>();

        for (DateTime d = start.Date; d < end.Date; d = d.AddDays(1))
        {
            labels.Add(d.Day.ToString());

            decimal sVal = salesByDay.ContainsKey(d) ? salesByDay[d] : 0m;
            decimal fVal = factoryByDay.ContainsKey(d) ? factoryByDay[d] : 0m;
            decimal distProfitVal = distProfitByDay.ContainsKey(d) ? distProfitByDay[d] : 0m;
            decimal benzVal = benzineByDay.ContainsKey(d) ? benzineByDay[d] : 0m;

            dailySales.Add(Math.Round(sVal, 3));
            dailyFactory.Add(Math.Round(fVal, 3));
            dailyDistNet.Add(Math.Round(distProfitVal - benzVal, 3));
        }

        DailyLabelsJson = JsonSerializer.Serialize(labels);
        DailySalesJson = JsonSerializer.Serialize(dailySales);
        DailyFactoryProfitJson = JsonSerializer.Serialize(dailyFactory);
        DailyDistributorNetJson = JsonSerializer.Serialize(dailyDistNet);

        List<string> topLabels = TopBySales.Select(x => x.ProductNameEn).ToList();
        List<decimal> topSales = TopBySales.Select(x => x.SalesTotal).ToList();

        TopProductLabelsJson = JsonSerializer.Serialize(topLabels);
        TopProductSalesJson = JsonSerializer.Serialize(topSales);
    }
}

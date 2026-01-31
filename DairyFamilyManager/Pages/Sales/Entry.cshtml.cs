using DairyFamilyManager.Data;
using DairyFamilyManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Security.Claims;

namespace DairyFamilyManager.Pages.Sales;

[Authorize(Policy = "DataEntryOrAdmin")]
public class EntryModel : PageModel
{
    private readonly AppDbContext _db;

    public EntryModel(AppDbContext db)
    {
        _db = db;
    }

    [BindProperty(SupportsGet = true)] public string DateString { get; set; } = DateTime.Today.ToString("yyyy-MM-dd");
    [BindProperty(SupportsGet = true)] public long ClientId { get; set; }

    public List<Client> ClientOptions { get; set; } = new List<Client>();
    public List<Product> ProductOptions { get; set; } = new List<Product>();

    [BindProperty] public List<SaleLineVm> Lines { get; set; } = new List<SaleLineVm>();

    public decimal TotalSales { get; set; }
    public decimal TotalFactoryProfit { get; set; }
    public decimal TotalDistributorProfit { get; set; }
    public bool ClientUsesDistributor { get; set; }

    public string Error { get; set; } = "";
    public string Message { get; set; } = "";

    public class SaleLineVm
    {
        public long ProductId { get; set; }
        public string? QuantityText { get; set; }
        public string? UnitPriceText { get; set; }
        public bool SaveAsDefault { get; set; }

        public string? UnitPrice { get; set; }
        public string? Quantity { get; set; }
    }

    public async Task OnGetAsync()
    {
        await LoadOptionsAsync();

        if (ClientId <= 0) return;

        bool ok = DateTime.TryParseExact(DateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
        if (!ok)
        {
            Error = "Invalid date.";
            return;
        }
        Client? client = await _db.Clients.AsNoTracking().FirstOrDefaultAsync(x => x.Id == ClientId);
        ClientUsesDistributor = client != null && client.UsesDistributor;



        DailySale? sale = await _db.DailySales
            .Include(x => x.Lines)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.ClientId == ClientId && x.Date == date.Date);

        if (sale == null)
        {
            Lines = new List<SaleLineVm> { new SaleLineVm() };
            await AutofillPricesAsync(date.Date);
            return;
        }

        Lines = sale.Lines.Select(l => new SaleLineVm
        {
            ProductId = l.ProductId,
            QuantityText = l.Quantity.ToString("0.###", CultureInfo.InvariantCulture),
            UnitPriceText = l.UnitPriceUsed.ToString("0.###", CultureInfo.InvariantCulture),
            SaveAsDefault = false
        }).ToList();

        if (Lines.Count == 0)
        {
            Lines.Add(new SaleLineVm());
            await AutofillPricesAsync(date.Date);
        }
        RecalcTotals(ClientUsesDistributor);

    }

    public async Task<IActionResult> OnPostAsync(string? action, int? removeIndex)
    {
        await LoadOptionsAsync();

        bool ok = DateTime.TryParseExact(DateString, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);
        if (!ok)
        {
            Error = "Invalid date.";
            return Page();
        }

        if (ClientId <= 0)
        {
            Error = "Select a client.";
            return Page();
        }

        if (removeIndex.HasValue)
        {
            int idx = removeIndex.Value;
            if (idx >= 0 && idx < Lines.Count)
            {
                Lines.RemoveAt(idx);
            }

            if (Lines.Count == 0) Lines.Add(new SaleLineVm());

            await AutofillPricesAsync(date.Date);

            return Page();
        }

        if (string.Equals(action, "add", StringComparison.OrdinalIgnoreCase))
        {
            Lines.Add(new SaleLineVm());
            await AutofillPricesAsync(date.Date);
            return Page();
        }

        if (!string.Equals(action, "save", StringComparison.OrdinalIgnoreCase))
        {
            await AutofillPricesAsync(date.Date);
            return Page();
        }

        long userId = 0;
        string? idStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (!string.IsNullOrWhiteSpace(idStr)) long.TryParse(idStr, out userId);

        Client? client = await _db.Clients.FirstOrDefaultAsync(x => x.Id == ClientId && x.IsActive);
        if (client == null)
        {
            Error = "Client not found or inactive.";
            return Page();
        }

        List<Product> products = await _db.Products.Where(x => x.IsActive).ToListAsync();

        DailySale? sale = await _db.DailySales.Include(x => x.Lines).FirstOrDefaultAsync(x => x.ClientId == ClientId && x.Date == date.Date);
        if (sale == null)
        {
            sale = new DailySale
            {
                ClientId = ClientId,
                Date = date.Date,
                CreatedByUserId = userId
            };
            _db.DailySales.Add(sale);
        }

        sale.Lines.Clear();

        for (int i = 0; i < Lines.Count; i++)
        {
            long productId = Lines[i].ProductId;
            Product? p = products.FirstOrDefault(x => x.Id == productId);
            if (p == null) continue;

            string qtyTxt = Lines[i].QuantityText == null ? "" : Lines[i].QuantityText.Trim();
            string priceTxt = Lines[i].UnitPriceText == null ? "" : Lines[i].UnitPriceText.Trim();

            if (string.IsNullOrWhiteSpace(qtyTxt)) continue;

            bool qtyOk = decimal.TryParse(qtyTxt, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal qty);
            if (!qtyOk || qty < 0)
            {
                Error = "Invalid quantity. Use numbers like 1 or 0.45 or 2.125";
                return Page();
            }

            decimal unitPrice;
            if (string.IsNullOrWhiteSpace(priceTxt))
            {
                unitPrice = await GetDefaultUnitPriceAsync(ClientId, productId, p.BasePrice);
            }
            else
            {
                bool priceOk = decimal.TryParse(priceTxt, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal parsed);
                if (!priceOk || parsed < 0)
                {
                    Error = "Invalid price. Use numbers like 8 or 8.5 or 8.125";
                    return Page();
                }
                unitPrice = parsed;
            }

            DailySaleLine line = new DailySaleLine
            {
                ProductId = productId,
                Quantity = qty,
                UnitPriceUsed = unitPrice,
                FactoryProfitTypeUsed = p.FactoryProfitType,
                FactoryProfitValueUsed = p.FactoryProfitValue,
                DistributorProfitTypeUsed = p.DistributorProfitType,
                DistributorProfitValueUsed = p.DistributorProfitValue
            };

            sale.Lines.Add(line);

            if (Lines[i].SaveAsDefault)
            {
                await UpsertClientPriceAsync(ClientId, productId, unitPrice);
            }
        }

        await _db.SaveChangesAsync();

        Message = "Saved.";
        return Redirect("/Sales/Entry?date=" + DateString + "&clientId=" + ClientId);
    }

    private async Task LoadOptionsAsync()
    {
        ClientOptions = await _db.Clients.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.NameEn).ToListAsync();
        ProductOptions = await _db.Products.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.NameEn).ToListAsync();
    }

    private async Task AutofillPricesAsync(DateTime date)
    {
        if (ClientId <= 0) return;

        List<ClientProductPrice> custom = await _db.ClientProductPrices.AsNoTracking().Where(x => x.ClientId == ClientId).ToListAsync();

        for (int i = 0; i < Lines.Count; i++)
        {
            if (Lines[i].ProductId <= 0) continue;

            if (!string.IsNullOrWhiteSpace(Lines[i].UnitPriceText)) continue;

            Product? p = ProductOptions.FirstOrDefault(x => x.Id == Lines[i].ProductId);
            if (p == null) continue;

            ClientProductPrice? found = custom.FirstOrDefault(x => x.ProductId == p.Id);
            decimal price = found == null ? p.BasePrice : found.Price;

            Lines[i].UnitPriceText = price.ToString("0.###", CultureInfo.InvariantCulture);

            Client? client = await _db.Clients.AsNoTracking().FirstOrDefaultAsync(x => x.Id == ClientId);
            bool usesDistributor = client != null && client.UsesDistributor;
            RecalcTotals(usesDistributor);

        }
    }

    private async Task<decimal> GetDefaultUnitPriceAsync(long clientId, long productId, decimal basePrice)
    {
        ClientProductPrice? found = await _db.ClientProductPrices.AsNoTracking().FirstOrDefaultAsync(x => x.ClientId == clientId && x.ProductId == productId);
        if (found != null) return found.Price;
        return basePrice;
    }

    private async Task UpsertClientPriceAsync(long clientId, long productId, decimal price)
    {
        ClientProductPrice? existing = await _db.ClientProductPrices.FirstOrDefaultAsync(x => x.ClientId == clientId && x.ProductId == productId);
        if (existing == null)
        {
            _db.ClientProductPrices.Add(new ClientProductPrice { ClientId = clientId, ProductId = productId, Price = price });
            return;
        }

        existing.Price = price;
    }
    private decimal CalcProfitPerUnit(ProfitType type, decimal value, decimal unitPrice)
    {
        if (type == ProfitType.Percent) return unitPrice * value / 100m;
        return value;
    }

    private void RecalcTotals(bool clientUsesDistributor)
    {
        ClientUsesDistributor = clientUsesDistributor;

        decimal totalSales = 0m;
        decimal totalFactory = 0m;
        decimal totalDistributor = 0m;

        for (int i = 0; i < Lines.Count; i++)
        {
            if (Lines[i].ProductId <= 0) continue;

            bool qtyOk = decimal.TryParse(Lines[i].QuantityText == null ? "" : Lines[i].QuantityText.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal qty);
            bool priceOk = decimal.TryParse(Lines[i].UnitPriceText == null ? "" : Lines[i].UnitPriceText.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price);

            if (!qtyOk || !priceOk) continue;
            if (qty <= 0m || price < 0m) continue;

            Product? p = ProductOptions.FirstOrDefault(x => x.Id == Lines[i].ProductId);
            if (p == null) continue;

            decimal lineTotal = price * qty;

            decimal factoryPerUnit = CalcProfitPerUnit(p.FactoryProfitType, p.FactoryProfitValue, price);
            decimal distributorPerUnit = CalcProfitPerUnit(p.DistributorProfitType, p.DistributorProfitValue, price);

            totalSales += lineTotal;
            totalFactory += factoryPerUnit * qty;

            if (clientUsesDistributor)
            {
                totalDistributor += distributorPerUnit * qty;
            }
        }

        TotalSales = Math.Round(totalSales, 3);
        TotalFactoryProfit = Math.Round(totalFactory, 3);
        TotalDistributorProfit = Math.Round(totalDistributor, 3);
    }

}

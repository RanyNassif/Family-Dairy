using DairyFamilyManager.Data;
using DairyFamilyManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DairyFamilyManager.Pages.Clients;

[Authorize(Policy = "AdminOnly")]
public class PricesModel : PageModel
{
    private readonly AppDbContext _db;

    public PricesModel(AppDbContext db)
    {
        _db = db;
    }

    public Client? Client { get; set; }

    [BindProperty] public long ClientId { get; set; }

    [BindProperty] public List<ClientPriceRow> Rows { get; set; } = new List<ClientPriceRow>();

    public string Error { get; set; } = "";
    public string Message { get; set; } = "";

    public class ClientPriceRow
    {
        public long ProductId { get; set; }
        public string ProductNameEn { get; set; } = "";
        public string ProductNameAr { get; set; } = "";
        public decimal BasePrice { get; set; }
        public string? CustomPriceText { get; set; }
    }

    public async Task<IActionResult> OnGetAsync(long id)
    {
        ClientId = id;

        Client = await _db.Clients.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
        if (Client == null) return Page();

        List<Product> products = await _db.Products.AsNoTracking().Where(x => x.IsActive).OrderBy(x => x.NameEn).ToListAsync();
        List<ClientProductPrice> custom = await _db.ClientProductPrices.AsNoTracking().Where(x => x.ClientId == id).ToListAsync();

        Rows = products.Select(p =>
        {
            ClientProductPrice? found = custom.FirstOrDefault(x => x.ProductId == p.Id);
            return new ClientPriceRow
            {
                ProductId = p.Id,
                ProductNameEn = p.NameEn,
                ProductNameAr = p.NameAr,
                BasePrice = p.BasePrice,
                CustomPriceText = found == null ? "" : found.Price.ToString("0.###", CultureInfo.InvariantCulture)
            };
        }).ToList();

        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        Client = await _db.Clients.FirstOrDefaultAsync(x => x.Id == ClientId);
        if (Client == null)
        {
            Error = "Client not found.";
            return Page();
        }

        List<Product> products = await _db.Products.AsNoTracking().Where(x => x.IsActive).ToListAsync();

        for (int i = 0; i < Rows.Count; i++)
        {
            long productId = Rows[i].ProductId;

            Product? p = products.FirstOrDefault(x => x.Id == productId);
            if (p == null) continue;

            string txt = Rows[i].CustomPriceText == null ? "" : Rows[i].CustomPriceText.Trim();
            bool hasValue = !string.IsNullOrWhiteSpace(txt);

            ClientProductPrice? existing = await _db.ClientProductPrices.FirstOrDefaultAsync(x => x.ClientId == ClientId && x.ProductId == productId);

            if (!hasValue)
            {
                if (existing != null)
                {
                    _db.ClientProductPrices.Remove(existing);
                }
                continue;
            }

            bool parsed = decimal.TryParse(txt, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal customPrice);
            if (!parsed || customPrice < 0)
            {
                Error = "Invalid price value. Use numbers like 8 or 8.5 or 8.125";
                return Page();
            }

            if (existing == null)
            {
                ClientProductPrice n = new ClientProductPrice
                {
                    ClientId = ClientId,
                    ProductId = productId,
                    Price = customPrice
                };
                _db.ClientProductPrices.Add(n);
            }
            else
            {
                existing.Price = customPrice;
            }
        }

        await _db.SaveChangesAsync();

        Message = "Saved.";
        return Redirect("/Clients/Prices?id=" + ClientId);
    }
}

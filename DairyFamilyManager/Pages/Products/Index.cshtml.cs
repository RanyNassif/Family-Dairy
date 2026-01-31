using DairyFamilyManager.Data;
using DairyFamilyManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DairyFamilyManager.Pages.Products;

[Authorize(Policy = "AdminOnly")]
public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db)
    {
        _db = db;
    }

    public List<Product> Products { get; set; } = new List<Product>();

    public async Task OnGetAsync()
    {
        Products = await _db.Products.AsNoTracking().OrderBy(x => x.NameEn).ToListAsync();
    }
}

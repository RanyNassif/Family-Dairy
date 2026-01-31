using DairyFamilyManager.Data;
using DairyFamilyManager.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace DairyFamilyManager.Pages.Clients;

[Authorize(Policy = "AdminOnly")]
public class IndexModel : PageModel
{
    private readonly AppDbContext _db;

    public IndexModel(AppDbContext db)
    {
        _db = db;
    }

    public List<Client> Clients { get; set; } = new List<Client>();

    public async Task OnGetAsync()
    {
        Clients = await _db.Clients.AsNoTracking().OrderBy(x => x.NameEn).ToListAsync();
    }
}

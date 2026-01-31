using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DairyFamilyManager.Pages;

[Authorize(Policy = "DataEntryOrAdmin")]
public class IndexModel : PageModel
{
    public void OnGet() { }
}

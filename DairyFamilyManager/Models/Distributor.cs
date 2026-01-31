namespace DairyFamilyManager.Models;

public class Distributor
{
    public long Id { get; set; }

    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}

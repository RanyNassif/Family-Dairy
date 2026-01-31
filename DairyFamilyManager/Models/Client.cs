namespace DairyFamilyManager.Models;

public class Client
{
    public long Id { get; set; }

    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;

    public bool UsesDistributor { get; set; }

    public bool IsActive { get; set; } = true;
}

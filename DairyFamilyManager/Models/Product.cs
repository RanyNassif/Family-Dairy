namespace DairyFamilyManager.Models;

public class Product
{
    public long Id { get; set; }

    public string NameEn { get; set; } = string.Empty;
    public string NameAr { get; set; } = string.Empty;

    public string? LabelEn { get; set; }
    public string? LabelAr { get; set; }

    public decimal BasePrice { get; set; }

    public ProfitType FactoryProfitType { get; set; } = ProfitType.Fixed;
    public decimal FactoryProfitValue { get; set; }

    public ProfitType DistributorProfitType { get; set; } = ProfitType.Fixed;
    public decimal DistributorProfitValue { get; set; }

    public bool IsActive { get; set; } = true;
}

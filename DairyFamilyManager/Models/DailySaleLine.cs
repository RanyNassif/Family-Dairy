namespace DairyFamilyManager.Models;

public class DailySaleLine
{
    public long Id { get; set; }

    public long DailySaleId { get; set; }
    public DailySale? DailySale { get; set; }

    public long ProductId { get; set; }
    public Product? Product { get; set; }

    public decimal Quantity { get; set; }
    public decimal UnitPriceUsed { get; set; }

    public ProfitType FactoryProfitTypeUsed { get; set; }
    public decimal FactoryProfitValueUsed { get; set; }

    public ProfitType DistributorProfitTypeUsed { get; set; }
    public decimal DistributorProfitValueUsed { get; set; }
}

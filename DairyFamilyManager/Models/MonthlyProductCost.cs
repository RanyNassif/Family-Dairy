namespace DairyFamilyManager.Models;

public class MonthlyProductCost
{
    public long Id { get; set; }

    public int Year { get; set; }
    public int Month { get; set; }

    public long ProductId { get; set; }
    public Product? Product { get; set; }

    public decimal MilkCost { get; set; }
    public decimal WorkersCost { get; set; }
    public decimal GasCost { get; set; }
    public decimal OtherCost { get; set; }
}
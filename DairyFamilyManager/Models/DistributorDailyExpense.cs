namespace DairyFamilyManager.Models;

public class DistributorDailyExpense
{
    public long Id { get; set; }

    public DateTime Date { get; set; }

    public long DistributorId { get; set; }
    public Distributor? Distributor { get; set; }

    public decimal BenzineAmount { get; set; }
}

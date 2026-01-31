namespace DairyFamilyManager.Models;

public class DailySale
{
    public long Id { get; set; }

    public DateTime Date { get; set; }

    public long ClientId { get; set; }
    public Client? Client { get; set; }

    public long CreatedByUserId { get; set; }

    public DateTime CreatedAtUtc { get; set; }

    public List<DailySaleLine> Lines { get; set; } = new List<DailySaleLine>();
}

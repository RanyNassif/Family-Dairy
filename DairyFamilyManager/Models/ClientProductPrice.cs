namespace DairyFamilyManager.Models;

public class ClientProductPrice
{
    public long Id { get; set; }

    public long ClientId { get; set; }
    public Client? Client { get; set; }

    public long ProductId { get; set; }
    public Product? Product { get; set; }

    public decimal Price { get; set; }
}


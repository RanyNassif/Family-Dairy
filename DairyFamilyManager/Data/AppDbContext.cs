using DairyFamilyManager.Models;
using Microsoft.EntityFrameworkCore;

namespace DairyFamilyManager.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    public DbSet<AppUser> Users => Set<AppUser>();

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<ClientProductPrice> ClientProductPrices => Set<ClientProductPrice>();
    public DbSet<DailySale> DailySales => Set<DailySale>();
    public DbSet<DailySaleLine> DailySaleLines => Set<DailySaleLine>();
    public DbSet<Distributor> Distributors => Set<Distributor>();
    public DbSet<DistributorDailyExpense> DistributorDailyExpenses => Set<DistributorDailyExpense>();

    public DbSet<MonthlyProductCost> MonthlyProductCosts => Set<MonthlyProductCost>();



    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AppUser>(e =>
        {
            e.ToTable("Users");
            e.HasKey(x => x.Id);
            e.Property(x => x.Username).HasMaxLength(100).IsRequired();
            e.HasIndex(x => x.Username).IsUnique();
            e.Property(x => x.PasswordHash).HasMaxLength(500).IsRequired();
            e.Property(x => x.Role).HasMaxLength(30).IsRequired();
            e.Property(x => x.IsActive).HasDefaultValue(true);
            e.Property(x => x.CreatedAtUtc).HasDefaultValueSql("GETUTCDATE()");
        });
        modelBuilder.Entity<Product>(e =>
        {
            e.ToTable("Products");
            e.HasKey(x => x.Id);

            e.Property(x => x.NameEn).HasMaxLength(200).IsRequired();
            e.Property(x => x.NameAr).HasMaxLength(200).IsRequired();

            e.Property(x => x.LabelEn).HasMaxLength(200);
            e.Property(x => x.LabelAr).HasMaxLength(200);

            e.Property(x => x.BasePrice).HasColumnType("decimal(18,3)").IsRequired();

            e.Property(x => x.FactoryProfitType).HasConversion<int>().IsRequired();
            e.Property(x => x.FactoryProfitValue).HasColumnType("decimal(18,3)").IsRequired();

            e.Property(x => x.DistributorProfitType).HasConversion<int>().IsRequired();
            e.Property(x => x.DistributorProfitValue).HasColumnType("decimal(18,3)").IsRequired();

            e.Property(x => x.IsActive).HasDefaultValue(true);

            e.HasIndex(x => x.NameEn);
        });
        modelBuilder.Entity<Client>(e =>
        {
            e.ToTable("Clients");
            e.HasKey(x => x.Id);

            e.Property(x => x.NameEn).HasMaxLength(200).IsRequired();
            e.Property(x => x.NameAr).HasMaxLength(200).IsRequired();

            e.Property(x => x.UsesDistributor).HasDefaultValue(false);
            e.Property(x => x.IsActive).HasDefaultValue(true);

            e.HasIndex(x => x.NameEn);
        });

        modelBuilder.Entity<ClientProductPrice>(e =>
        {
            e.ToTable("ClientProductPrices");
            e.HasKey(x => x.Id);

            e.Property(x => x.Price).HasColumnType("decimal(18,3)").IsRequired();

            e.HasOne(x => x.Client)
                .WithMany()
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(x => new { x.ClientId, x.ProductId }).IsUnique();
        });

        modelBuilder.Entity<DailySale>(e =>
        {
            e.ToTable("DailySales");
            e.HasKey(x => x.Id);

            e.Property(x => x.Date).HasColumnType("date").IsRequired();

            e.Property(x => x.CreatedAtUtc).HasDefaultValueSql("GETUTCDATE()").IsRequired();

            e.HasOne(x => x.Client)
                .WithMany()
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasMany(x => x.Lines)
                .WithOne(x => x.DailySale)
                .HasForeignKey(x => x.DailySaleId)
                .OnDelete(DeleteBehavior.Cascade);

            e.HasIndex(x => new { x.Date, x.ClientId }).IsUnique();
        });

        modelBuilder.Entity<DailySaleLine>(e =>
        {
            e.ToTable("DailySaleLines");
            e.HasKey(x => x.Id);

            e.Property(x => x.Quantity).HasColumnType("decimal(18,3)").IsRequired();
            e.Property(x => x.UnitPriceUsed).HasColumnType("decimal(18,3)").IsRequired();

            e.Property(x => x.FactoryProfitTypeUsed).HasConversion<int>().IsRequired();
            e.Property(x => x.FactoryProfitValueUsed).HasColumnType("decimal(18,3)").IsRequired();

            e.Property(x => x.DistributorProfitTypeUsed).HasConversion<int>().IsRequired();
            e.Property(x => x.DistributorProfitValueUsed).HasColumnType("decimal(18,3)").IsRequired();

            e.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => new { x.DailySaleId, x.ProductId });
        });
        modelBuilder.Entity<Distributor>(e =>
        {
            e.ToTable("Distributors");
            e.HasKey(x => x.Id);

            e.Property(x => x.NameEn).HasMaxLength(200).IsRequired();
            e.Property(x => x.NameAr).HasMaxLength(200).IsRequired();
            e.Property(x => x.IsActive).HasDefaultValue(true);

            e.HasIndex(x => x.NameEn);
        });

        modelBuilder.Entity<DistributorDailyExpense>(e =>
        {
            e.ToTable("DistributorDailyExpenses");
            e.HasKey(x => x.Id);

            e.Property(x => x.Date).HasColumnType("date").IsRequired();
            e.Property(x => x.BenzineAmount).HasColumnType("decimal(18,3)").IsRequired();

            e.HasOne(x => x.Distributor)
                .WithMany()
                .HasForeignKey(x => x.DistributorId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => new { x.Date, x.DistributorId }).IsUnique();
        });
        modelBuilder.Entity<MonthlyProductCost>(e =>
        {
            e.ToTable("MonthlyProductCosts");
            e.HasKey(x => x.Id);

            e.Property(x => x.Year).IsRequired();
            e.Property(x => x.Month).IsRequired();

            e.Property(x => x.MilkCost).HasColumnType("decimal(18,3)").IsRequired();
            e.Property(x => x.WorkersCost).HasColumnType("decimal(18,3)").IsRequired();
            e.Property(x => x.GasCost).HasColumnType("decimal(18,3)").IsRequired();
            e.Property(x => x.OtherCost).HasColumnType("decimal(18,3)").IsRequired();

            e.HasOne(x => x.Product)
                .WithMany()
                .HasForeignKey(x => x.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            e.HasIndex(x => new { x.Year, x.Month, x.ProductId }).IsUnique();
        });

    }
}

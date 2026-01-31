using DairyFamilyManager.Data;
using DairyFamilyManager.Models;
using Microsoft.EntityFrameworkCore;

namespace DairyFamilyManager.Services;

public class SeedService
{
    private readonly AppDbContext _db;
    private readonly PasswordService _passwords;

    public SeedService(AppDbContext db, PasswordService passwords)
    {
        _db = db;
        _passwords = passwords;
    }

    public async Task SeedUsersAsync()
    {
        await _db.Database.MigrateAsync();

        bool any = await _db.Users.AnyAsync();
        bool hasDistributor = await _db.Set<Distributor>().AnyAsync();
        if (!hasDistributor)
        {
            _db.Set<Distributor>().Add(new Distributor
            {
                NameEn = "Main Distributor",
                NameAr = "الموزع الرئيسي",
                IsActive = true
            });

            await _db.SaveChangesAsync();
        }

        if (any) return;

        AppUser admin = new AppUser
        {
            Username = "rnassif",
            PasswordHash = _passwords.HashPassword("ChangeMe123!"),
            Role = "Admin",
            IsActive = true
        };

        AppUser dad = new AppUser
        {
            Username = "dad",
            PasswordHash = _passwords.HashPassword("ChangeMe123!"),
            Role = "DataEntry",
            IsActive = true
        };

        _db.Users.Add(admin);
        _db.Users.Add(dad);
        await _db.SaveChangesAsync();
    }
}

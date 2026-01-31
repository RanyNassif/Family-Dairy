using DairyFamilyManager.Data;
using DairyFamilyManager.Models;
using Microsoft.EntityFrameworkCore;

namespace DairyFamilyManager.Services;

public class AuthService
{
    private readonly AppDbContext _db;
    private readonly PasswordService _passwords;

    public AuthService(AppDbContext db, PasswordService passwords)
    {
        _db = db;
        _passwords = passwords;
    }

    public async Task<AppUser?> ValidateAsync(string username, string password)
    {
        if (string.IsNullOrWhiteSpace(username)) return null;
        if (string.IsNullOrWhiteSpace(password)) return null;

        string u = username.Trim();

        AppUser? user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Username == u && x.IsActive);
        if (user == null) return null;

        bool ok = _passwords.VerifyPassword(password, user.PasswordHash);
        if (!ok) return null;

        return user;
    }
}

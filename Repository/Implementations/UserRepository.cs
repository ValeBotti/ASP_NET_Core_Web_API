using Microsoft.EntityFrameworkCore;
public class UserRepository : IUserRepository
{
/// <remarks>
/// OVERVIEW: This repository performs persistence operations for the user entity.
/// </remarks>
    private readonly AppDbContext _db;

    public UserRepository(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Retrieves user information by their identifier.
    /// </summary>
    /// <remarks>
    /// EFFECTS:
    /// Queries the database and returns the user associated with the specified identifier.
    /// </remarks>
    public async Task<User?> GetUserAsync(int uid)
    {
        return await _db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == uid);
    }
    
    /// <summary>
    /// Persists a new User and its associated session identifier.
    /// </summary>
    /// <remarks>
    ///
    /// MODIFIES:
    /// Inserts a new User record and a new UidSid record into the database.
    ///
    /// EFFECTS:
    /// Generates a session identifier, associates it with the newly created user,
    /// and returns both identifiers.
    /// throws an exception if the user is not found or if the card expiration date or card number is invalid.
    /// </remarks>
    public async Task UpdateUserAsync(User user)
    {
        try
        {
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException?.Message.Contains("CK_user_card_expire_month") == true)
                throw new InvalidOperationException("INVALID_CARD_EXPIRE_MONTH");

            if (ex.InnerException?.Message.Contains("CK_user_id_positive") == true)
                throw new InvalidOperationException("INVALID_USER_ID");

            throw;
        }
    }
}
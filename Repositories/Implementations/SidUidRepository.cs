public class SidUidRepository : ISessionRepository
{
/// <remarks>
/// OVERVIEW: This repository performs persistence operations for the uid_sid entity.
/// </remarks>

    private readonly AppDbContext _db;

    public SidUidRepository(AppDbContext db)
    {
        _db = db;
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
    /// </remarks>
    public async Task<(int uid, string sid)> CreateSessionAsync()
    {
        var user = new User
        {
            FirstName = null,
            LastName = null,
            CardFullName = null,
            CardNumber = null,
            CardExpireMonth = null,
            CardExpireYear = null,
            CardCVV = null
        };

        _db.Users.Add(user);
        await _db.SaveChangesAsync();

        string id = Guid.NewGuid().ToString();

        var uidSid = new UidSid
        {
            Id = id,
            Uid = user.Id
        };

        _db.UidSids.Add(uidSid);
        await _db.SaveChangesAsync();

        return (user.Id, id);
    }
}

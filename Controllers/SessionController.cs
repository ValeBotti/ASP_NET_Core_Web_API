using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SessionController : ControllerBase
{
/// <summary>
/// Provides HTTP endpoints for managing user's sessions.
/// </summary>
/// <remarks>
/// OVERVIEW: This controller exposes operations for creating a user session.
/// </remarks>

    private readonly AppDbContext _db;

    public SessionController(AppDbContext db)
    {
        _db = db;
    }
    
    /// <summary>
    /// Creates a user and an associated session.
    /// </summary>
    /// <remarks>
    ///
    /// MODIFIES:
    /// The database state by inserting a new user and an associated session.
    /// 
    /// EFFECTS:
    /// Creates a new user and an associated session, and returns their identifiers.
    /// </remarks>
    [HttpPost("create")]
    public IActionResult CreateSession()
    {

        var user = new User
        {
            FirstName = null,
            LastName = null,
            CardFullName = null,
            CardNumber = null,
            CardExpireMonth =  null,
            CardExpireYear = null,
            CardCVV = null
        };

        _db.Users.Add(user);
        _db.SaveChanges();

        string sid = Guid.NewGuid().ToString();

        var uidSid = new UidSid
        {
            Sid = sid,
            Uid = user.Uid
        };

        _db.UidSids.Add(uidSid);
        _db.SaveChanges();

        return Ok(new
        {
            sid = sid,
            uid = user.Uid
        });
    }
}
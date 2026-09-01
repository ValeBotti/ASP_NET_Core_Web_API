using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
/// <summary>
/// Provides HTTP endpoints for managing users.
/// </summary>
/// <remarks>
/// OVERVIEW: This controller exposes operations for managing user accounts.
/// </remarks>

    private readonly AppDbContext _db;

    public UserController(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <remarks>
    /// 
    /// EFFECTS:
    /// Retrieves and returns the user associated with the specified unique identifier.
    /// </remarks>
    [HttpGet("{uid}")]
    public IActionResult GetUser(int uid)
    {
        var user = _db.Users.FirstOrDefault(u => u.Uid == uid);
        if (user == null) return NotFound();

        return Ok(user);
    }

    /// <summary>
    /// Updates a user's information.
    /// </summary>
    /// <remarks>
    ///
    /// MODIFIES:
    /// The database state by updating the user's information.
    /// 
    /// EFFECTS:
    /// Updates a user's information and returns the updated user's information.
    /// </remarks>
    [HttpPut("{uid}")]
    public IActionResult UpdateUser(int uid, [FromBody] UpdateUserBody dto)
    {

        var user = _db.Users.FirstOrDefault(u => u.Uid == uid);
        if (user == null) return NotFound();

        user.FirstName = dto.FirstName ?? user.FirstName;
        user.LastName = dto.LastName ?? user.LastName;

        user.CardFullName = dto.CardFullName ?? user.CardFullName;
        user.CardNumber = dto.CardNumber ?? user.CardNumber;
        user.CardExpireMonth = dto.CardExpireMonth ?? user.CardExpireMonth;
        user.CardExpireYear = dto.CardExpireYear ?? user.CardExpireYear;
        user.CardCVV = dto.CardCVV ?? user.CardCVV;

        //user.LastOid = dto.LastOid ?? user.LastOid;
        //user.OrderStatus = dto.OrderStatus ?? user.OrderStatus;

        _db.SaveChanges();

        return Ok(user);
    }
}
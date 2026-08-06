using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly AppDbContext _db;

    public UserController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("{uid}")]
    public IActionResult GetUser(int uid)
    {
        var user = _db.Users.FirstOrDefault(u => u.Uid == uid);
        if (user == null) return NotFound();

        return Ok(user);
    }

    [HttpPut("{uid}")]
    public IActionResult UpdateUser(int uid, [FromBody] UpdateUserDto dto)
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

        user.LastOid = dto.LastOid ?? user.LastOid;
        user.OrderStatus = dto.OrderStatus ?? user.OrderStatus;

        _db.SaveChanges();

        return Ok(user);
    }
}
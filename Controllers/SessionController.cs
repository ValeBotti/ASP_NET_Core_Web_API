using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class SessionController : ControllerBase
{
    private readonly AppDbContext _db;

    public SessionController(AppDbContext db)
    {
        _db = db;
    }
    
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
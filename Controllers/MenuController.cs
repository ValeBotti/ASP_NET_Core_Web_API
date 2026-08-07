using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
    private readonly AppDbContext _db;

    public MenuController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet("")]
    public IActionResult GetMenu()
    {
        var menu = _db.Menus
            .AsNoTracking()
            .Select(m => new MenuListDto
            {
                Mid = m.Mid,
                Name = m.Name,
                Price = m.Price,
                Location = m.Location,
                ImageVersion = m.ImageVersion,
                ShortDescription = m.ShortDescription,
                LongDescription = m.LongDescription,
                DeliveryTime = m.DeliveryTime
            })
            .ToList();

        return Ok(menu);
    }

    [HttpGet("{mid}/image")]
    public IActionResult GetMenuImage(int mid)
    {
        var menu = _db.Menus.FirstOrDefault(m => m.Mid == mid);
        if (menu == null)
            return NotFound();

        return Ok(new { base64 = menu.Image });// Return the image data as a JSON object
    }
    
    [HttpGet("{mid}")]
    public IActionResult GetMenuDetails(int mid)
    {
        var menu = _db.Menus
            .AsNoTracking()
            .Where(m => m.Mid == mid)
            .Select(m => new MenuListDto
            {
                Mid = m.Mid,
                Name = m.Name,
                Price = m.Price,
                Location = m.Location,
                ImageVersion = m.ImageVersion,
                ShortDescription = m.ShortDescription,
                LongDescription = m.LongDescription,
                DeliveryTime = m.DeliveryTime
            })
            .FirstOrDefault();

        if (menu == null)
            return NotFound();

        return Ok(menu);
    }
}
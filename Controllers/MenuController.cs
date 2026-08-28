using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class MenuController : ControllerBase
{
/// <summary>
/// Provides HTTP endpoints for managing menus.
/// </summary>
/// <remarks>
/// OVERVIEW: This controller exposes operations for retrieving menu information.
/// </remarks>

    private readonly AppDbContext _db;

    public MenuController(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Retrieves the list of available menus.
    /// </summary>
    /// <remarks>
    /// 
    /// EFFECTS:
    /// Retrieves and returns all menus in the database.
    /// </remarks>
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

    /// <summary>
    /// Retrieves a menu's image.
    /// </summary>
    /// 
    /// <remarks>
    /// EFFECTS:
    /// Retrieves and returns the image associated with the specified menu identifier.
    /// </remarks>
    [HttpGet("{mid}/image")]
    public IActionResult GetMenuImage(int mid)
    {
        var menu = _db.Menus.FirstOrDefault(m => m.Mid == mid);
        if (menu == null)
            return NotFound();

        return Ok(new { base64 = menu.Image });// Return the image data as a JSON object
    }
    
    /// <summary>
    /// Retrieves a menu's details.
    /// </summary>
    /// 
    /// <remarks>
    /// EFFECTS:
    /// Retrieves and returns the details associated with the specified menu identifier.
    /// </remarks>
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
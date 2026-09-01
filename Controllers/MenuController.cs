using Microsoft.AspNetCore.Mvc;

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
///
 
    private readonly IMenuRepository _menuRepository;

    private readonly AppDbContext _db;

    public MenuController(IMenuRepository menuRepository, AppDbContext db)
    {
        _menuRepository = menuRepository;
        _db = db;
    }

    /// <summary>
    /// GET /api/menu
    /// Returns a list of menus.
    /// </summary>
    [HttpGet("")]
    public IActionResult GetMenu()
    {
        var menu = _menuRepository.GetMenu();
        return Ok(menu);
    }

    /// <summary>
    /// GET /api/menu/{mid}/image
    /// Returns the image associated with the specified menu identifier.
    /// </summary>
    [HttpGet("{mid}/image")]
    public IActionResult GetMenuImage(int mid)
    {
        var exists = _db.Menus.Any(m => m.Id == mid);
        if (!exists)
            return NotFound();

        var base64 = _menuRepository.GetMenuImage(mid);
        return Ok(new { base64 });
    }

    /// <summary>
    /// GET /api/menu/{mid}
    /// Returns the details associated with the specified menu identifier.
    /// </summary>
    [HttpGet("{mid}")]
    public IActionResult GetMenuDetails(int mid)
    {
        var exists = _db.Menus.Any(m => m.Id == mid);
        if (!exists)
            return NotFound();

        var dto = _menuRepository.GetMenuDetails(mid);
        return Ok(dto);
    }
}
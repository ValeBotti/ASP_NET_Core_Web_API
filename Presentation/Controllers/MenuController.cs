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

    public MenuController(IMenuRepository menuRepository)
    {
        _menuRepository = menuRepository;
    }

    /// <summary>
    /// GET /api/menu
    /// Returns a list of menus.
    /// </summary>
    [HttpGet("")]
    public async Task<IActionResult> GetMenu()
    {
        var menu = await _menuRepository.GetMenuAsync();
        return Ok(menu);
    }

    /// <summary>
    /// GET /api/menu/{mid}/image
    /// Returns the image associated with the specified menu identifier.
    /// </summary>
    [HttpGet("{mid}/image")]
    public async Task<IActionResult> GetMenuImage(int mid)
    {
        var base64 = await _menuRepository.GetMenuImageAsync(mid);
        return Ok(new { base64 });
    }

    /// <summary>
    /// GET /api/menu/{mid}
    /// Returns the details associated with the specified menu identifier.
    /// </summary>
    [HttpGet("{mid}")]
    public async Task<IActionResult> GetMenuDetails(int mid)
    {
        var dto = await _menuRepository.GetMenuDetailsAsync(mid);
        return Ok(dto);
    }
}
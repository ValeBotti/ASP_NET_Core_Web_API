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

    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// GET /api/user/{uid}
    /// Returns the user associated with the specified unique identifier.
    /// </remarks>
    [HttpGet("{uid}")]
    public async Task<IActionResult> GetUser(int uid)
    {
        var user = await _userService.GetUserAsync(uid);
        return Ok(user);
    }

    /// <summary>
    /// PUT /api/user/{uid}
    /// Updates a user's information and returns them.
    /// </summary>
    [HttpPut("{uid}")]
    public async Task<IActionResult> UpdateUser(int uid, [FromBody] UserDto dto)
    {
        var updated = await _userService.UpdateUserAsync(uid, dto);
        return Ok(updated);
    }

}
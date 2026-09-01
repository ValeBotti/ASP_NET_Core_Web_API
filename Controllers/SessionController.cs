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

    private readonly ISessionRepository _sessionRepository;

    public SessionController(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }


    /// <summary>
    /// POST /api/session/create
    /// Creates a user and an associated session.
    /// </summary>
    /// <remarks>
    /// 
    /// EFFECTS:
    /// Returns the generated session identifier and the user identifier.
    /// </remarks>
    [HttpPost("create")]
    public async Task<IActionResult> CreateSession()
    {
        var (uid, sid) = await _sessionRepository.CreateSessionAsync();

        return Ok(new
        {
            uid,
            sid
        });
    }

}
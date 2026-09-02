using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

[ApiController]
public class ErrorController : ControllerBase
{
    [Route("/error")]
    public IActionResult HandleError()
    {
        var exception = HttpContext.Features.Get<IExceptionHandlerFeature>()?.Error;

        return exception switch
        {
            InvalidOperationException ex => BadRequest(new { error = ex.Message }),
            KeyNotFoundException ex => NotFound(new { error = ex.Message }),
            _ => StatusCode(500, new { error = "Errore interno del server" })
        };
    }
}

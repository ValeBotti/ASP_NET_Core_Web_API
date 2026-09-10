using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[ApiExplorerSettings(IgnoreApi = true)]
public class ErrorController : ControllerBase
{
    [Route("/error")]
    public IActionResult HandleError()
    {
        var exception = HttpContext.Features
            .Get<IExceptionHandlerFeature>()?.Error;

        return exception switch
        {
            UnauthorizedAccessException ex =>
                StatusCode(403, new { error = ex.Message }),

            InvalidOperationException ex when ex.Message == "ORDER_ALREADY_ON_DELIVERY" =>
                Conflict(new { error = ex.Message }),

            InvalidOperationException ex =>
                BadRequest(new { error = ex.Message }),

            KeyNotFoundException ex =>
                NotFound(new { error = ex.Message }),

            _ =>
                StatusCode(500, new { error = "Errore interno del server" })
        };
    }
}

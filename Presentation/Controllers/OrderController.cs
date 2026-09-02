using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
/// <summary>
/// Provides HTTP endpoints for managing orders.
/// </summary>
/// <remarks>
/// OVERVIEW: This controller exposes operations for managing orders.
/// </remarks>
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    /// <summary>
    /// POST /api/order/{mid}/buy
    /// Purchases an order for the menu item with the specified unique identifier.
    /// </summary>
    [HttpPost("{mid}/buy")]
    public async Task<IActionResult> PostOrder(int mid, [FromBody] BuyOrderBody body)
    {
        var orderBought = await _orderService.CreateOrderAsync(mid, body);

        return Ok(orderBought);
    }


    /// <summary>
    /// GET /api/order/{oid}
    /// Retrieves the current state of the order with the specified unique identifier.
    /// </summary>
    [HttpGet("{oid}")]
    public async Task<IActionResult> GetCurrentOrder(int oid)
    {
        var order = await _orderService.GetCurrentOrderAsync(oid);

        return Ok(order);
    }
}
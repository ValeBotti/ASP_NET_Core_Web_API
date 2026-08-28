using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
/// <summary>
/// Provides HTTP endpoints for managing orders.
/// </summary>
/// <remarks>
/// OVERVIEW: This controller exposes operations for creating an order and retrieving the object's status.
/// </remarks>

    private readonly AppDbContext _db;

    public OrderController(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Purchases an order.
    /// </summary>
    /// <remarks>
    ///
    /// MODIFIES:
    /// The database state by inserting a new order and updating the associated
    /// user's order information.
    /// 
    /// EFFECTS:
    /// Creates an order for the user associated with the request, updates the user's order information, and returns the created order information.
    /// </remarks>

    [HttpPost("{mid}/buy")]
    public IActionResult PostOrder(int mid, [FromBody] BuyOrderBody body)
    {
        var menu = _db.Menus
            .AsNoTracking()
            .FirstOrDefault(m => m.Mid == mid);

        if (menu == null)
            return NotFound($"Menu con MID {mid} non trovato.");

        string cardNumber = body.CardNumber;

        if (!string.IsNullOrEmpty(cardNumber) && cardNumber.StartsWith("0"))
        {
            return StatusCode(403, "INVALID_CARD");
        }

        string sid = body.Sid;
        Location deliveryLocation = body.DeliveryLocation;

        var uidEntry = _db.UidSids
            .AsNoTracking()
            .FirstOrDefault(u => u.Sid == sid);

        if (uidEntry == null)
            return NotFound($"SID {sid} non trovato nella tabella UidSid.");

        int uid = uidEntry.Uid;

        Location menuLocation = menu.Location;
        int deliveryTime = menu.DeliveryTime;

        var order = new Order
        {
            Mid = mid,
            Uid = uid,
            CreationTimestamp = DateTime.UtcNow.ToString("o"),
            Status = "ON_DELIVERY",
            DeliveryLocation = new Location
            {
                Lat = deliveryLocation.Lat,
                Lng = deliveryLocation.Lng
            },
            ExpectedDeliveryTimestamp = DateTime.UtcNow.AddMinutes(menu.DeliveryTime).ToString("o"),
            DeliveryTimestamp = DateTime.UtcNow.AddMinutes(menu.DeliveryTime).ToString("o"),
            CurrentPosition = new Location
            {
                Lat = deliveryLocation.Lat,
                Lng = deliveryLocation.Lng
            }
        };

        try {
            _db.Orders.Add(order);
            _db.SaveChanges();
        } catch (DbUpdateException ex) {
            if (ex.InnerException?.Message.Contains("IX_Orders_Uid_OnDelivery") == true) {
                return StatusCode(409, "ORDER_ALREADY_ON_DELIVERY");
            }
            throw;
        }

        var dto = new OrderBoughtDto
        {
            Oid = order.Oid,
            Uid = order.Uid,
            Mid = order.Mid,
            CreationTimestamp = order.CreationTimestamp,
            Status = order.Status,
            DeliveryLocation = order.DeliveryLocation,
            ExpectedDeliveryTimestamp = order.ExpectedDeliveryTimestamp,
            CurrentPosition = order.CurrentPosition
        };

        var user = _db.Users.FirstOrDefault(u => u.Uid == uid);

        if (user == null)
            return NotFound($"Utente con UID {uid} non trovato.");

        user.LastOid = order.Oid;
        user.OrderStatus = order.Status;

        _db.SaveChanges();

        return Ok(dto);
    }

    /// <summary>
    /// Retrieves the current state of an order.
    /// </summary>
    /// <remarks>
    ///
    /// MODIFIES:
    /// The order state and the associated user's order information in the database
    /// when the delivery status changes.
    /// 
    /// EFFECTS:
    /// Retrieves and determines the current delivery status and position of the order.
    /// </remarks>

    [HttpGet("{oid}")]
    public IActionResult GetCurrentOrder(int oid)
    {
        var order = _db.Orders.FirstOrDefault(o => o.Oid == oid);
        if (order == null)
            return NotFound($"Order con OID {oid} non trovato.");

        var menu = _db.Menus.FirstOrDefault(m => m.Mid == order.Mid);
        if (menu == null)
            return NotFound($"Menu con MID {order.Mid} non trovato.");

        var user = _db.Users.FirstOrDefault(u => u.Uid == order.Uid);
        if (user == null)
            return NotFound($"Utente con UID {order.Uid} non trovato.");

        var creation = DateTime.Parse(order.CreationTimestamp, null, System.Globalization.DateTimeStyles.RoundtripKind).ToUniversalTime();
        var delivery = creation.AddMinutes(menu.DeliveryTime);
        var now = DateTime.UtcNow;

        bool isCompleted = now >= delivery;
        string status = isCompleted ? "COMPLETED" : "ON_DELIVERY";

        float totalSeconds = (float)(delivery - creation).TotalSeconds;
        float elapsedSeconds = (float)(now - creation).TotalSeconds;

        float progress = totalSeconds > 0 ? elapsedSeconds / totalSeconds : 1f;
        progress = Math.Clamp(progress, 0f, 1f);

        var currentPosition = new Location
        {
            Lat = menu.Location.Lat + (order.CurrentPosition.Lat - menu.Location.Lat) * progress,
            Lng = menu.Location.Lng + (order.CurrentPosition.Lng - menu.Location.Lng) * progress
        };

        bool statusChanged = order.Status != status;
        if (statusChanged)
        {
            order.Status = status;
            order.DeliveryTimestamp = delivery.ToString("o");
            _db.Orders.Update(order);
            _db.SaveChanges();

            user.LastOid = order.Oid;
            user.OrderStatus = order.Status;

            _db.SaveChanges();
        }

        if (isCompleted)
        {
            return Ok(new OrderCompletedDto
                {
                    Oid = order.Oid,
                    Uid = order.Uid,
                    Mid = order.Mid,
                    CreationTimestamp = order.CreationTimestamp,
                    Status = status,
                    DeliveryLocation = order.DeliveryLocation,
                    DeliveryTimestamp = delivery.ToString("o"),
                    CurrentPosition = currentPosition
                });
        }
        else 
        {
            return Ok(new OrderOnDeliveryDto
                {
                    Oid = order.Oid,
                    Uid = order.Uid,
                    Mid = order.Mid,
                    CreationTimestamp = order.CreationTimestamp,
                    Status = status,
                    DeliveryLocation = order.DeliveryLocation,
                    ExpectedDeliveryTimestamp = delivery.ToString("o"),
                    CurrentPosition = currentPosition
                });
        }
    }
}
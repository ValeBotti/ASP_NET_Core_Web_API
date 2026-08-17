using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class OrderController : ControllerBase
{
    private readonly AppDbContext _db;

    public OrderController(AppDbContext db)
    {
        _db = db;
    }

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

        return Ok(dto);
    }

    [HttpGet("{oid}")]
    public IActionResult GetOrder(int oid)
    {
        var order = _db.Orders
            .FirstOrDefault(o => o.Oid == oid);

        if (order == null)
            return NotFound($"Order con OID {oid} non trovato.");

        var creation = DateTime.Parse(order.CreationTimestamp);
        var expected = DateTime.Parse(order.ExpectedDeliveryTimestamp);
        var now = DateTime.UtcNow;

        float totalSeconds = (float)(expected - creation).TotalSeconds;
        float elapsedSeconds = (float)(now - creation).TotalSeconds;

        float progress = elapsedSeconds / totalSeconds;
        progress = Math.Clamp(progress, 0f, 1f);

        float latNow =
            order.CurrentPosition.Lat +
            (order.DeliveryLocation.Lat - order.CurrentPosition.Lat) * progress;

        float lngNow =
            order.CurrentPosition.Lng +
            (order.DeliveryLocation.Lng - order.CurrentPosition.Lng) * progress;

        var dronePosition = new Location
        {
            Lat = latNow,
            Lng = lngNow
        };

        string deliveryTimestamp;
        string status;

        if (now >= expected)
        {
            deliveryTimestamp = order.ExpectedDeliveryTimestamp;
            status = "COMPLETED";
        }
        else
        {
            deliveryTimestamp = now.ToString("o");
            status = "ON_DELIVERY";
        }

        order.CurrentPosition = dronePosition;
        order.DeliveryTimestamp = deliveryTimestamp;
        order.Status = status;

        _db.Orders.Update(order);
        _db.SaveChanges();

        var dto = new OrderUpdateDto
        {
            Oid = order.Oid,
            Uid = order.Uid,
            Mid = order.Mid,
            CreationTimestamp = order.CreationTimestamp,
            Status = order.Status,
            DeliveryLocation = order.DeliveryLocation,
            DeliveryTimestamp = order.DeliveryTimestamp,
            CurrentPosition = order.CurrentPosition
        };

        return Ok(dto);
    }

}
using Microsoft.EntityFrameworkCore;

public class OrderService //: IOrderService
{
    private readonly AppDbContext _db;

    public OrderService(AppDbContext db)
    {
        _db = db;
    }
/*
    // ------------------------------------------------------------
    // CREATE ORDER (POST /api/Order/{mid}/buy)
    // ------------------------------------------------------------
    public async Task<OrderBoughtDto> CreateOrderAsync(int mid, BuyOrderBody body)
    {
        // 1. Recupero menu
        var menu = await _db.Menus
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Mid == mid);

        if (menu == null)
            throw new KeyNotFoundException($"Menu con MID {mid} non trovato.");

        // 2. Validazione carta
        if (!string.IsNullOrEmpty(body.CardNumber) &&
            body.CardNumber.StartsWith("0"))
        {
            throw new InvalidOperationException("INVALID_CARD");
        }

        // 3. Recupero UID tramite SID
        var uidEntry = await _db.UidSids
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Sid == body.Sid);

        if (uidEntry == null)
            throw new KeyNotFoundException($"SID {body.Sid} non trovato nella tabella UidSid.");

        int uid = uidEntry.Uid;

        // 4. Creazione ordine
        var order = new Order
        {
            Mid = mid,
            Uid = uid,
            CreationTimestamp = DateTime.UtcNow.ToString("o"),
            Status = "ON_DELIVERY",
            DeliveryLocation = new Location
            {
                Lat = body.DeliveryLocation.Lat,
                Lng = body.DeliveryLocation.Lng
            },
            ExpectedDeliveryTimestamp = DateTime.UtcNow.AddMinutes(menu.DeliveryTime).ToString("o"),
            DeliveryTimestamp = DateTime.UtcNow.AddMinutes(menu.DeliveryTime).ToString("o"),
            CurrentPosition = new Location
            {
                Lat = body.DeliveryLocation.Lat,
                Lng = body.DeliveryLocation.Lng
            }
        };

        // 5. Salvataggio ordine
        try
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException?.Message.Contains("IX_Orders_Uid_OnDelivery") == true)
                throw new InvalidOperationException("ORDER_ALREADY_ON_DELIVERY");

            throw;
        }

        // 6. Aggiornamento utente
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Uid == uid);

        if (user == null)
            throw new KeyNotFoundException($"Utente con UID {uid} non trovato.");

        user.LastOid = order.Oid;
        user.OrderStatus = order.Status;

        await _db.SaveChangesAsync();

        // 7. Costruzione DTO
        return new OrderBoughtDto
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
    }

    // ------------------------------------------------------------
    // GET CURRENT ORDER (GET /api/Order/{oid})
    // ------------------------------------------------------------
    public async Task<OrderDtoBase> GetCurrentOrderAsync(int oid)
    {
        // 1. Recupero ordine
        var order = await _db.Orders.FirstOrDefaultAsync(o => o.Oid == oid);
        if (order == null)
            throw new KeyNotFoundException($"Order con OID {oid} non trovato.");

        // 2. Recupero menu
        var menu = await _db.Menus.FirstOrDefaultAsync(m => m.Mid == order.Mid);
        if (menu == null)
            throw new KeyNotFoundException($"Menu con MID {order.Mid} non trovato.");

        // 3. Recupero utente
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Uid == order.Uid);
        if (user == null)
            throw new KeyNotFoundException($"Utente con UID {order.Uid} non trovato.");

        // 4. Calcolo tempi
        var creation = DateTime.Parse(order.CreationTimestamp, null,
            System.Globalization.DateTimeStyles.RoundtripKind).ToUniversalTime();

        var delivery = creation.AddMinutes(menu.DeliveryTime);
        var now = DateTime.UtcNow;

        bool isCompleted = now >= delivery;
        string status = isCompleted ? "COMPLETED" : "ON_DELIVERY";

        // 5. Calcolo progresso
        float totalSeconds = (float)(delivery - creation).TotalSeconds;
        float elapsedSeconds = (float)(now - creation).TotalSeconds;

        float progress = totalSeconds > 0 ? elapsedSeconds / totalSeconds : 1f;
        progress = Math.Clamp(progress, 0f, 1f);

        // 6. Calcolo posizione drone
        var currentPosition = new Location
        {
            Lat = menu.Location.Lat + (order.DeliveryLocation.Lat - menu.Location.Lat) * progress,
            Lng = menu.Location.Lng + (order.DeliveryLocation.Lng - menu.Location.Lng) * progress
        };

        // 7. Aggiornamento stato ordine
        bool statusChanged = order.Status != status;
        if (statusChanged)
        {
            order.Status = status;
            order.DeliveryTimestamp = delivery.ToString("o");
            order.CurrentPosition = currentPosition;

            _db.Orders.Update(order);

            user.LastOid = order.Oid;
            user.OrderStatus = order.Status;

            await _db.SaveChangesAsync();
        }

        // 8. Costruzione DTO corretto
        if (isCompleted)
        {
            return new OrderCompletedDto
            {
                Oid = order.Oid,
                Uid = order.Uid,
                Mid = order.Mid,
                CreationTimestamp = order.CreationTimestamp,
                Status = status,
                DeliveryLocation = order.DeliveryLocation,
                DeliveryTimestamp = delivery.ToString("o"),
                CurrentPosition = currentPosition
            };
        }

        return new OrderOnDeliveryDto
        {
            Oid = order.Oid,
            Uid = order.Uid,
            Mid = order.Mid,
            CreationTimestamp = order.CreationTimestamp,
            Status = status,
            DeliveryLocation = order.DeliveryLocation,
            ExpectedDeliveryTimestamp = delivery.ToString("o"),
            CurrentPosition = currentPosition
        };
    }*/
}


public class OrderService : IOrderService
{

    private readonly IOrderRepository _repo;

    private readonly IMenuRepository _menuRepo;

    private readonly ISessionRepository _sessionRepo;

    private readonly IOrderRepository _orderRepo;

    private readonly IUserRepository _userRepo;

    public OrderService(IOrderRepository repo, IMenuRepository menuRepo, ISessionRepository sessionRepo, IOrderRepository orderRepo, IUserRepository userRepo)
    {
        _repo = repo;
        _menuRepo = menuRepo;
        _sessionRepo = sessionRepo;
        _orderRepo = orderRepo;
        _userRepo = userRepo;
    }
    
    /// <summary>
    /// Purchases an order.
    /// </summary>
    /// <remarks>
    /// 
    /// EFFECTS:
    /// Creates a new order in the database, updates the user's order information, and returns the created order information.
    /// Throws exceptions if the menu item or user is not found, or if the card number is invalid.
    /// </remarks>
    public async Task<OrderOnDeliveryDto> CreateOrderAsync(int mid, BuyOrderBody body)
    {
        var menu = await _menuRepo.GetMenuByIdAsync(mid);
        if (menu == null)
            throw new KeyNotFoundException($"Menu con MID {mid} non trovato.");

        if (!string.IsNullOrEmpty(body.CardNumber) && body.CardNumber.StartsWith("0"))
            throw new InvalidOperationException("INVALID_CARD");

        var uidEntry = await _sessionRepo.GetUidBySidAsync(body.Sid);
        if (uidEntry == null)
            throw new KeyNotFoundException($"SID {body.Sid} non trovato.");

        int uid = uidEntry.Uid;

        var order = new Order
        {
            Mid = mid,
            Uid = uid,
            CreationTimestamp = DateTime.UtcNow.ToString("o"),
            Status = "ON_DELIVERY",
            DeliveryTimestamp = null
        };

        try
        {
            await _repo.AddOrderAsync(order);
        }
        catch (InvalidOperationException ex) when (ex.Message == "ORDER_ALREADY_ON_DELIVERY")
        {
            throw new InvalidOperationException("ORDER_ALREADY_ON_DELIVERY");
        }

        return new OrderOnDeliveryDto
        {
            Oid = order.Id,
            Uid = order.Uid,
            Mid = order.Mid,
            CreationTimestamp = order.CreationTimestamp,
            Status = order.Status,
            DeliveryLocation = menu.Location,
            ExpectedDeliveryTimestamp = DateTime.UtcNow.AddMinutes(menu.DeliveryTime).ToString("o"),
            CurrentPosition = body.DeliveryLocation
        };
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
    public async Task<OrderDtoBase> GetCurrentOrderAsync(int oid, Location UserLocation)
    {
        var order = await _orderRepo.GetCurrentOrderAsync(oid);
        if (order == null)
            throw new KeyNotFoundException($"Order con OID {oid} non trovato.");

        var menu = await _menuRepo.GetMenuByIdAsync(order.Mid);
        if (menu == null)
            throw new KeyNotFoundException($"Menu con MID {order.Mid} non trovato.");

        var user = await _userRepo.GetUserAsync(order.Uid);
        if (user == null)
            throw new KeyNotFoundException($"Utente con UID {order.Uid} non trovato.");

        var creation = DateTime.Parse(order.CreationTimestamp, null, System.Globalization.DateTimeStyles.RoundtripKind).ToUniversalTime();

        var delivery = creation.AddMinutes(menu.DeliveryTime);
        var now = DateTime.UtcNow;

        bool isCompleted = now >= delivery;
        order.Status = isCompleted ? "COMPLETED" : "ON_DELIVERY";

        order.DeliveryTimestamp = isCompleted ? delivery.ToString("o") : null;

        float totalSeconds = (float)(delivery - creation).TotalSeconds;
        float elapsedSeconds = (float)(now - creation).TotalSeconds;

        float progress = totalSeconds > 0 ? elapsedSeconds / totalSeconds : 1f;
        progress = Math.Clamp(progress, 0f, 1f);

        var currentPosition = new Location
        {
            Lat = menu.Location.Lat + (UserLocation.Lat - menu.Location.Lat) * progress,

            Lng = menu.Location.Lng + (UserLocation.Lng - menu.Location.Lng) * progress
        };

        try
        {
            await _orderRepo.UpdateOrderAsync(order);
        }
        catch (KeyNotFoundException)
        {
            throw new KeyNotFoundException($"Order con OID {order.Id} non trovato.");
        }

        if (isCompleted)
        {
            return new OrderCompletedDto
            {
                Oid = order.Id,
                Uid = order.Uid,
                Mid = order.Mid,
                CreationTimestamp = order.CreationTimestamp,
                Status = order.Status,
                DeliveryLocation = UserLocation,
                DeliveryTimestamp = delivery.ToString("o"),
                CurrentPosition = currentPosition
            };
        }

        return new OrderOnDeliveryDto
        {
            Oid = order.Id,
            Uid = order.Uid,
            Mid = order.Mid,
            CreationTimestamp = order.CreationTimestamp,
            Status = order.Status,
            DeliveryLocation = UserLocation,
            ExpectedDeliveryTimestamp = delivery.ToString("o"),
            CurrentPosition = currentPosition
        };
    }

}

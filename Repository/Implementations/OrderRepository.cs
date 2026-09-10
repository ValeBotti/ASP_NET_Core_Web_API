using Microsoft.EntityFrameworkCore;
public class OrderRepository : IOrderRepository
{
    private readonly AppDbContext _db;

    public OrderRepository(AppDbContext db)
    {
        _db = db;
    }

    /// <summary>
    /// Retrieves an order by the user's unique identifier (UID) from the database.
    /// </summary>
    /// <remarks>
    /// 
    /// EFFECTS:
    /// Returns the most recent order associated with the specified UID, or null if no order is found.
    /// </remarks>
    public async Task<Order?> GetLastOrderAsync(int uid)
        {
            if (!await _db.Users.AnyAsync(u => u.Id == uid))
                throw new KeyNotFoundException($"User con UID {uid} non trovato.");

            return await _db.Orders
                .AsNoTracking()
                .Where(o => o.Uid == uid)
                .OrderByDescending(o => o.Id)
                .FirstOrDefaultAsync();
        }

    /// <summary>
    /// Inserts a new order into the database.
    /// </summary>
    /// <remarks>
    /// 
    /// EFFECTS:
    /// Adds a record of a new order to the database and saves the changes.
    /// </remarks>
    public async Task AddOrderAsync(Order order)
    {
        try
        {
            _db.Orders.Add(order);
            await _db.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException?.Message.Contains("IX_order_user_id_OnDelivery") == true)
                throw new InvalidOperationException("ORDER_ALREADY_ON_DELIVERY");

            throw;
        }
    }

    /// <summary>
    /// Retrieves a selected order by its unique identifier (OID) from the database.
    /// </summary>
    /// <remarks>
    /// 
    /// EFFECTS:
    /// Returns the order associated with the specified OID, or null if no order is found.
    /// </remarks>
    public async Task<Order?> GetCurrentOrderAsync(int oid)
    {
        var order = await _db.Orders
            .AsNoTracking()
            .FirstOrDefaultAsync(o => o.Id == oid);

        if (order == null)
            throw new KeyNotFoundException($"Order con OID {oid} non trovato.");

        return order;
    }

    /// <summary>
    /// Changes the status and current position of an existing order in the database.
    /// </summary>
    /// <remarks>
    /// 
    /// EFFECTS:
    /// Updates the status and current position of the specified order in the database and saves the changes.
    /// </remarks>
    public async Task UpdateOrderAsync(Order order)
    {
        var existingOrder = await _db.Orders.FindAsync(order.Id);
        if (existingOrder == null)
            throw new KeyNotFoundException($"Order con OID {order.Id} non trovato.");

        existingOrder.Status = order.Status;
        existingOrder.DeliveryTimestamp = order.DeliveryTimestamp;

        await _db.SaveChangesAsync();
    }
}


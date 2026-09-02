public interface IOrderRepository
{
    Task<Order?> GetLastOrderAsync(int uid);
    Task AddOrderAsync(Order order);
    Task<Order?> GetCurrentOrderAsync(int oid);
    Task UpdateOrderAsync(Order order);
}

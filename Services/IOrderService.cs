public interface IOrderService
{
    Task<OrderBoughtDto> CreateOrderAsync(int mid, BuyOrderBody body);
    Task<OrderDtoBase> GetCurrentOrderAsync(int oid);
}
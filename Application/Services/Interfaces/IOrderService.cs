public interface IOrderService
{
    Task<OrderOnDeliveryDto> CreateOrderAsync(int mid, BuyOrderBody body);
    Task<OrderDtoBase> GetCurrentOrderAsync(int oid);
    
}
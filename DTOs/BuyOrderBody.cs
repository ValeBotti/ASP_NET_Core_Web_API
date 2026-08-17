public class BuyOrderBody
{
    public required string Sid { get; set; }
    public required string CardNumber { get; set; }
    public required Location DeliveryLocation { get; set; }
}
public class BuyOrderBody
{
/// <summary>
/// OVERVIEW: Represents the request body with all the necessary information for purchasing an order. It's custom.
/// </summary>

    public required string Sid { get; set; }
    public required string CardNumber { get; set; }
    public required Location DeliveryLocation { get; set; }
}
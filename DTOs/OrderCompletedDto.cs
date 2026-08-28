public class OrderCompletedDto
{
    public required int Oid { get; set; }
    public required int Uid { get; set; }
    public required int Mid { get; set; }
    public required string CreationTimestamp { get; set; }
    public required string Status { get; set; }
    public required Location DeliveryLocation { get; set; }
    public required Location CurrentPosition { get; set; }
    public required string DeliveryTimestamp { get; set; }
    
}
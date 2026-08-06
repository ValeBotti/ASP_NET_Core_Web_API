using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    [Key]
    public required int Oid { get; set; }

    [ForeignKey(nameof(User))]
    public required int Uid { get; set; }
    public User? User { get; set; }

    [ForeignKey(nameof(Menu))]
    public required int Mid { get; set; }
    public Menu? Menu { get; set; }

    public required string CreationTimestamp { get; set; }

    public required string Status { get; set; }

    public required Location DeliveryLocation { get; set; }

    public required string ExpectedDeliveryTimestamp { get; set; }

    public required string DeliveryTimestamp { get; set; }

    public required Location CurrentPosition { get; set; }

}
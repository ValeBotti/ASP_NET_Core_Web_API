using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    [Key]
    public int Id { get; set; }

    [ForeignKey(nameof(User))]
    public required int Uid { get; set; }
    public virtual User? User { get; set; }

    [ForeignKey(nameof(Menu))]
    public required int Mid { get; set; }
    public virtual Menu? Menu { get; set; }

    public required string CreationTimestamp { get; set; }

    public required string Status { get; set; }

    public string? DeliveryTimestamp { get; set; }

    public required Location CurrentPosition { get; set; }

}
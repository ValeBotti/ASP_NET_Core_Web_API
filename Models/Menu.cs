using System.ComponentModel.DataAnnotations;

public class Menu
{
    [Key]
    public required int Mid { get; set; }

    public required string Name { get; set; }

    public required double Price { get; set; }

    public required Location Location { get; set; }

    public required int ImageVersion { get; set; }

    public required string ShortDescription { get; set; }

    public required string LongDescription { get; set; }

    public required int DeliveryTime { get; set; }

}
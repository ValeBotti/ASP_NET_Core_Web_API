using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UidSid
{
    [Key]
    public required string Id { get; set; }

    [ForeignKey(nameof(User))]
    public required int Uid { get; set; }
    public virtual User? User { get; set; } // navigation property
}
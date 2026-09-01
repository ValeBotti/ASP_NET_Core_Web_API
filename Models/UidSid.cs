using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class UidSid
{
    [Key]
    public string Id { get; set; } = default!;

    [ForeignKey(nameof(User))]
    public required int Uid { get; set; }
    public virtual User? User { get; set; } // navigation property
}
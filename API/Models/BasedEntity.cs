using System.ComponentModel.DataAnnotations.Schema;
using API.Enums;

namespace API.Models;

public abstract class BasedEntity<TId>
{
    [Column(TypeName = "varchar(8)")]
    public TId Id { get; set; }

    public Status Status { get; set; } = Status.Active;

    public DateTime? CreatedOn { get; set; } = DateTime.UtcNow;
    public DateTime? LastModifiedOn { get; set; }
}
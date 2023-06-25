using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class TableOrder : Audit
{
    [Required] public Guid TableId { get; set; }
    [Required] public Guid ProductId { get; set; }
    [Required] public int Quantity { get; set; }
    public Guid? SessionId { get; set; }
}
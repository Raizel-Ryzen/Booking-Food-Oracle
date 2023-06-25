using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class TableOrderRequest : Audit
{
    [Required] public Guid TableId { get; set; }
    [Required] public string Message { get; set; }
    public Guid? SessionId { get; set; }
}
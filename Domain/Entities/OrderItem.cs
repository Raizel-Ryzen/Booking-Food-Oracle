using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class OrderItem : Audit
{
    [Required] public int Quantity { get; set; }
    [Required] public double TotalAmount { set; get; }
    [Required] public Guid OrderId { get; set; }
    [Required] public Guid ProductId { get; set; }
}
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Payment : Audit
{
    [Required] public Guid OrderId { get; set; }
    [Required] public double TotalAmount { get; set; }
    [Required] public string PaymentMethod { get; set; }
    [Required] public string TransactionId { get; set; }
    [Required] public string PaymentCode { get; set; }
    [Required] public DateTime TransactionDate { set; get; }
    [Required] public int Status { get; set; }
}
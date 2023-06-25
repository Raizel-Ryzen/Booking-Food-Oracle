using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Order : Audit
{
    [Required] public string Code { get; set; }
    [Required] public string CustomerName { get; set; }
    [Required] public string Address { get; set; }
    [Required] public string PhoneNumber { get; set; }
    [Required] public double TotalAmount { get; set; }
    [Required] public int Status { get; set; }
}
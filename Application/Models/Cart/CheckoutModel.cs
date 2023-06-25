using System.ComponentModel.DataAnnotations;

namespace Application.Models.Cart;

public class CheckoutModel
{
    [Required] public string CustomerName { get; set; }
    [Required] public string Address { get; set; }
    [Required] public string PhoneNumber { get; set; }
    public double TotalAmount { get; set; }
    public int Status { get; set; }
}
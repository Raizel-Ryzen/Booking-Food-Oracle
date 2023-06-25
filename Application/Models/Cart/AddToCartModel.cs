using System.ComponentModel.DataAnnotations;

namespace Application.Models.Cart;

public class AddToCartModel
{
    [Required] public string Id { get; set; }
    [Required] public string Title { get; set; }
    [Required] public int Quantity { get; set; }
    [Required] public double Amount { get; set; }
    [Required] public string Image { get; set; }
    [Required] public string Unit { get; set; }
    [Required] public double Total { get; set; }
}

public class RemoveFromCartModel
{
    [Required] public string Id { get; set; }
}
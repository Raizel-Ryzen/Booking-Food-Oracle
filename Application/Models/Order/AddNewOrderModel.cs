using Application.Models.Cart;

namespace Application.Models.Order;

public class AddNewOrderModel
{
    public CheckoutModel CustomerInfo { get; set; }
    public List<AddToCartModel> ListCart { get; set; }
}
namespace Application.Models.Cart;

public class OrderSuccessModel
{
    public string Code { get; set; }
    public List<AddToCartModel> ListItems { get; set; }
}
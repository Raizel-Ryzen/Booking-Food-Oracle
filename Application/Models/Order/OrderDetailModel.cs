using Application.Common.Models;

namespace Application.Models.Order;

public class OrderDetailModel
{
    public OrderInfoDetailModel OrderInfo { get; set; }
    public List<OrderItemDetailModel> ListItems { get; set; }
    public ListDropdownStatusOrderModel ListDropdownStatus { get; set; }
}

public class OrderInfoDetailModel : AuditModel
{
    public string Code { get; set; }
    public string CustomerName { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public double TotalAmount { get; set; }
    public int Status { get; set; }
}

public class OrderItemDetailModel : AuditModel
{
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public double Amount { get; set; }
    public double TotalAmount { get; set; }
    public bool IsDeleted { get; set; }
}
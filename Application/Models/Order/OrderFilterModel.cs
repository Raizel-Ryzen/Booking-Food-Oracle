namespace Application.Models.Order;

public class OrderFilterModel
{
    public int? Status { get; set; }
    public string Title { get; set; }
    public int PageNumber { get; set; }
    public int RowOfPage { get; set; }
}
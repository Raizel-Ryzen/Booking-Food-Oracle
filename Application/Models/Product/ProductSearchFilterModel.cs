
namespace Application.Models.Product;

public class ProductSearchFilterModel
{
    public int? Status { get; set; }
    public string SearchText { get; set; }
    public int PageNumber { get; set; }
    public int RowOfPage { get; set; }
    public string OrderBy { get; set; }
    public string OrderType { get; set; }
    public int IsOrderByOnly { get; set; }
    public string CategoryId { get; set; }
}
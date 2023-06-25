using Application.Common.Models;
using Application.Models.Categories;

namespace Application.Models.Product;

public class ProductDetailModel : AuditModel
{
    public string Title { get; set; }
    public string Code { get; set; }
    public string Thumbnail { get; set; }
    public double Amount { get; set; }
    public int Status { get; set; }
    public int Bought { get; set; }
    public string Url { get; set; }
    public Guid CategoryId { get; set; }
    public Guid UnitId { get; set; }
    public string CategoryName { get; set; }
    public string UnitName { get; set; }
}
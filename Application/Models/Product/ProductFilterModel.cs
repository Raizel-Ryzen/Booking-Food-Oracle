using Application.Common.Models;

namespace Application.Models.Product;

public class ProductFilterModel
{
    public List<DropdownDetailModel> Categories { get; set; }
    public List<DropdownDetailModel> Units { get; set; }
    public ProductDetailModel Product { get; set; }
}
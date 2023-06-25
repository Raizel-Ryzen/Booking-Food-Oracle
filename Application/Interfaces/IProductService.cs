using Application.Common.Models;
using Application.Models.Product;

namespace Application.Interfaces;

public interface IProductService
{
    Task<bool> AddNew(AddNewProductModel model);
    Task<bool> Update(UpdateProductModel model);
    Task<bool> Delete(DeleteModel model);
    Task<ProductDetailModel> Detail(string id);
    Task<List<ProductDetailModel>> GetListProduct(ProductSearchFilterModel request);
    Task<bool> UpdateStatus(UpdateStatusProductModel model);
}
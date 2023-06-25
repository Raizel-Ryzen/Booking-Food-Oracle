using Application.Common.Models;
using Application.Models.Order;
using Application.Models.Table;

namespace Application.Interfaces;

public interface IOrderService
{
    Task<List<OrderInfoDetailModel>> GetListOrder(OrderFilterModel model);
    Task<OrderDetailModel> GetDetail(string code);
    Task<bool> Delete(DeleteModel model);
    Task<string> AddNew(AddNewOrderModel model);
    Task<bool> UpdateStatus(UpdateStatusOrderModel model);
}
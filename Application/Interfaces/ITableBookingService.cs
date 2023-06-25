using Application.Common.Models;
using Application.Models.Table;
using Application.Models.TableBooking;

namespace Application.Interfaces;

public interface ITableBookingService
{
    Task<List<TableBookingDetailModel>> GetListTableBooking(TableFilterModel model);
    Task<TableBookingDetailModel> GetDetail(string id);
    Task<bool> UpdateStatus(UpdateStatusTableBookingModel model);
    Task<string> AddNew(AddNewTableBookingModel model);
    Task<TableBookingDetailModel> GetDetailByCode(string code);
}
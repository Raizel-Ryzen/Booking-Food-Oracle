using Application.Common.Models;
using Application.Models.Table;

namespace Application.Interfaces;

public interface ITableService
{
    Task<List<TableDetailModel>> GetListTable(TableFilterModel model);
    Task<TableDetailModel> GetDetail(string id);
    Task<bool> Delete(DeleteModel model);
    Task<bool> AddNew(AddNewTableModel model);
    Task<bool> Update(UpdateTableModel model);
    Task<bool> UpdateStatus(UpdateStatusTableModel model);
    Task<bool> UpdateSessionId(UpdateTableSessionIdModel model);
    Task<TableDetailModel> GetDetailByCode(string code);
}
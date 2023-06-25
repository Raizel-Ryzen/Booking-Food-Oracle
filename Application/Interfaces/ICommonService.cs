using Application.Common.Models;
using Application.Models.Home;

namespace Application.Interfaces;

public interface ICommonService
{
    Task<DashboardAdminModel> GetDashboardAdmin();
    Task<List<DropdownDetailModel>> GetListDropdown(string action);
}
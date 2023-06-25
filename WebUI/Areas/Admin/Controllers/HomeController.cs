using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers;

public class HomeController : BaseAdminController
{
    private readonly ICommonService _commonService;

    public HomeController(ICommonService commonService)
    {
        _commonService = commonService;
    }

    public async Task<IActionResult> Index()
    {
        var data = await _commonService.GetDashboardAdmin();
        return View();
    }
}
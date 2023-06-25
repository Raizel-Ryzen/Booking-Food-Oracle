using Application.Common.Models;
using Application.Interfaces;
using Application.Models.Product;
using Application.Models.Table;
using Application.Models.TableBooking;
using Domain.Constants;
using Domain.Enums;
using Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using WebUI.WebHelper;

namespace WebUI.Areas.Admin.Controllers;

public class TableBookingController : BaseAdminController
{
    private readonly ICommonService _commonService;
    private readonly IGitService _gitService;
    private readonly ITableService _tableService;
    private readonly IQRCodeService _qrCodeService;
    private readonly ITableBookingService _tableBookingService;

    public TableBookingController(ICommonService commonService, IGitService gitService, ITableService tableService, IQRCodeService qrCodeService, ITableBookingService tableBookingService)
    {
        _commonService = commonService;
        _gitService = gitService;
        _tableService = tableService;
        _qrCodeService = qrCodeService;
        _tableBookingService = tableBookingService;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public IActionResult UpdateStatus([FromBody] UpdateStatusTableBookingModel request)
    {
        var dataModal = new ActionWithDialogModel<UpdateStatusTableBookingModel>()
        {
            Data = request,
            Title = "Cập Nhật Trạng Thái Yêu Cầu Đặt Bàn",
        };
        
        return PartialView("_UpdateStatus", dataModal);
    }

    [HttpGet]
    public async Task<IActionResult> GetListTableBooking([FromQuery] TableFilterModel request)
    {
        var products = await _tableBookingService.GetListTableBooking(request);
        var paging = PagingHelper<TableBookingDetailModel>
            .ToPaging(products, 1, int.MaxValue);

        return AppResponse(true, "Get Success", paging);
    }
    
    [HttpPut]
    public async Task<IActionResult> SubmitUpdateStatus([FromBody] UpdateStatusTableBookingModel request)
    {
        var result = await _tableBookingService.UpdateStatus(request);
        
        var msg = result
            ? "Lưu thành công."
            : "Có lỗi sảy ra. Vui lòng thử lại";
    
        return AppResponse(result, msg, result);
    }
}
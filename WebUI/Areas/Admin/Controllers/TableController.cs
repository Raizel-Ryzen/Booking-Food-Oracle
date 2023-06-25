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

public class TableController : BaseAdminController
{
    private readonly ICommonService _commonService;
    private readonly IGitService _gitService;
    private readonly ITableService _tableService;
    private readonly IQRCodeService _qrCodeService;
    private readonly ITableBookingService _tableBookingService;

    public TableController(ICommonService commonService, IGitService gitService, ITableService tableService, IQRCodeService qrCodeService, ITableBookingService tableBookingService)
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
    
    public IActionResult AddNewTable()
    {
        return View();
    }
    
    public async Task<IActionResult> Detail(string id)
    {
        var table = await _tableService.GetDetail(id);
        
        if (string.IsNullOrEmpty(table.Title))
        {
            return Redirect("/page-not-found");
        }
        
        return View(table);
    }
    
    [HttpPost]
    public IActionResult UpdateStatus([FromBody] UpdateStatusTableModel request)
    {
        var dataModal = new ActionWithDialogModel<UpdateStatusTableModel>()
        {
            Data = request,
            Title = "Cập Nhật Trạng Thái Bàn Ăn"
        };
        
        return PartialView("_UpdateStatus", dataModal);
    }
    
    [HttpPost]
    public IActionResult Delete([FromBody] DeleteModel request)
    {
        var dataModal = new ActionWithDialogModel<DeleteModel>()
        {
            Data = request,
            Title = "Bạn Có Chắc Muốn Xóa Không?"
        };
        
        return PartialView("_Delete", dataModal);
    }
    
    [HttpPut]
    public async Task<IActionResult> SubmitDelete([FromBody] DeleteModel request)
    {
        var result = await _tableService.Delete(request);
        
        var msg = result
            ? "Lưu thành công."
            : "Có lỗi sảy ra. Vui lòng thử lại";

        return AppResponse(result, msg, result);
    }
    
    [HttpPut]
    public async Task<IActionResult> SubmitUpdateStatus([FromBody] UpdateStatusTableModel request)
    {
        var result = await _tableService.UpdateStatus(request);
        
        var msg = result
            ? "Lưu thành công."
            : "Có lỗi sảy ra. Vui lòng thử lại";

        return AppResponse(result, msg, result);
    }
        
    [HttpGet]
    public async Task<IActionResult> GetListTable([FromQuery] TableFilterModel request)
    {
        var products = await _tableService.GetListTable(request);
        var paging = PagingHelper<TableDetailModel>
            .ToPaging(products, 1, int.MaxValue);

        return AppResponse(true, "Get Success", paging);
    }
    
    [HttpPost]
    public async Task<IActionResult> SubmitAddNewTable([FromBody] AddNewTableModel request)
    {
        var validation = ValidationHelper<AddNewTableModel>.IsValid(request);

        if (!validation.IsValid) return AppResponse(false, "Vui lòng điền đầy đủ thông tin");

        var code = Guid.NewGuid().ToString().Split('-').First().ToUpper();
        request.Code = code;

        var bookingTableUrl = AppConst.AppDomain + $"/table-info?code={code}";
        var qrCode = await _qrCodeService.GenerateQRCodeAsync(bookingTableUrl);
        request.QRCode = qrCode;
        var result = await _tableService.AddNew(request);

        var msg = result
            ? "Lưu thành công."
            : "Có lỗi sảy ra. Vui lòng thử lại";

        return AppResponse(result, msg, result);
    }
    
    [HttpPut]
    public async Task<IActionResult> SubmitUpdateTable([FromBody] UpdateTableModel request)
    {
        var validation = ValidationHelper<UpdateTableModel>.IsValid(request);

        if (!validation.IsValid) return AppResponse(false, "Vui lòng điền đầy đủ thông tin");

        var result = await _tableService.Update(request);

        var msg = result
            ? "Lưu thành công."
            : "Có lỗi sảy ra. Vui lòng thử lại";

        return AppResponse(result, msg, result);
    }
    
}
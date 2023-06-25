using Application.Interfaces;
using Application.Models.Categories;
using Application.Models.Product;
using Application.Models.Table;
using Application.Models.TableBooking;
using Domain.Constants;
using Domain.Enums;
using Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using WebUI.WebHelper;

namespace WebUI.Controllers;

public class TableController : BaseController
{
    private readonly ITableService _tableService;
    private readonly IWebService _webService;
    private readonly ITableBookingService _tableBookingService;

    public TableController(ITableService tableService, IWebService webService, ITableBookingService tableBookingService)
    {
        _tableService = tableService;
        _webService = webService;
        _tableBookingService = tableBookingService;
    }

    #region Do not use at this time

    public IActionResult GetListStatusLeftBar()
    {
        var data = new List<TableStatusModel>()
        {
            new TableStatusModel()
            {
                Code = "0",
                Title = "Tất Cả",
                Status = 0
            },
            new TableStatusModel()
            {
                Code = "1",
                Title = "Còn Trống",
                Status = (int)TableStatus.Available
            },
            new TableStatusModel()
            {
                Code = "2",
                Title = "Đã Được Đăt",
                Status = (int)TableStatus.NotAvailable
            }
        };
        return PartialView("_StatusListLeftBarPartial", data);
    }

    [HttpPost]
    public async Task<IActionResult> BookingTable([FromBody] TableFilterModel request)
    {
        var bookedExisting = _webService.GetStringSession(SessionKey.TableBooking);
        var sessionIdExisting = _webService.GetStringSession(SessionKey.SessionId);
        var tableIdExisting = _webService.GetStringSession(SessionKey.TableIdInSession);

        if (!string.IsNullOrEmpty(bookedExisting) || !string.IsNullOrEmpty(sessionIdExisting) ||
            !string.IsNullOrEmpty(tableIdExisting))
        {
            return AppResponse(false, "Bạn đã đặt bàn trước đó rồi.");
        }

        var table = await _tableService.GetDetail(request.Id);

        if (string.IsNullOrEmpty(request.Id) || string.IsNullOrEmpty(table.Code))
        {
            return AppResponse(false, "Đặt bàn thất bại.");
        }

        if (table.Status != (int)TableStatus.Available)
        {
            return AppResponse(false, "Bàn này đã có người đặt.");
        }

        var sessionId = Guid.NewGuid().ToString();

        _webService.SetStringSession(SessionKey.TableBooking, "Booked");
        _webService.SetStringSession(SessionKey.TableIdInSession, table.Id.ToString());
        _webService.SetStringSession(SessionKey.SessionId, sessionId);

        await _tableService.UpdateSessionId(new UpdateTableSessionIdModel()
        {
            Id = request.Id,
            SessionId = sessionId
        });

        await _tableService.UpdateStatus(new UpdateStatusTableModel()
        {
            Id = request.Id,
            Status = (int)TableStatus.NotAvailable
        });

        return AppResponse(true, "Đặt bàn thành công.");
    }

    [Route("/user-booking-table")]
    public async Task<IActionResult> UserBookingTable()
    {
        var booked = _webService.GetStringSession(SessionKey.TableBooking);
        var sessionId = _webService.GetStringSession(SessionKey.SessionId);
        var tableId = _webService.GetStringSession(SessionKey.TableIdInSession);

        if (string.IsNullOrEmpty(booked) || string.IsNullOrEmpty(sessionId) || string.IsNullOrEmpty(tableId))
        {
            return Redirect("/table");
        }

        var table = await _tableService.GetDetail(tableId);

        if (string.IsNullOrEmpty(table.Code) || table.Status == (int)TableStatus.Available)
        {
            return Redirect("/table");
        }

        return View(new UserBookingTableModel()
        {
            SessionId = sessionId,
            TableId = tableId,
            TableName = table.Title,
            QRCode = table.QRCode
        });
    }

    #endregion
    
    [Route("/table")]
    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetListTable([FromQuery] TableFilterModel request)
    {
        var tables = await _tableService.GetListTable(request);
        var paging = PagingHelper<TableDetailModel>
            .ToPaging(tables, 1, int.MaxValue);

        return PartialView("_TableListPartial", paging);
    }
    
    [HttpGet]
    public async Task<IActionResult> TableBookingInfo(string id)
    {
        var table = await _tableService.GetDetail(id);

        if (string.IsNullOrEmpty(table.Code) || table.Status != (int)TableStatus.Available)
        {
            return Redirect("/page-not-found");
        }

        return PartialView("_TableBookingInfoPartial", new UserBookingTableModel()
        {
            TableId = table.Id.ToString(),
            TableName = table.Title,
            QRCode = table.QRCode
        });
    }

    [HttpPost]
    public async Task<IActionResult> SubmitBookingTable([FromBody] AddNewTableBookingModel request)
    {
        var validation = ValidationHelper<AddNewTableBookingModel>.IsValid(request);

        if (!validation.IsValid) return AppResponse(false, "Vui lòng điền đầy đủ thông tin");
        
        var sessionId = Guid.NewGuid();

        _webService.SetStringSession(SessionKey.SessionId, sessionId.ToString());

        request.SessionId = sessionId;

        var result = await _tableBookingService.AddNew(request);

        return AppResponse(!string.IsNullOrEmpty(result),
            !string.IsNullOrEmpty(result) ? "Đặt bàn thành công" : "Đặt bàn thất bại", result);
    }
    
    [Route("booking-table-success")]
    [HttpGet]
    public IActionResult BookingTableSuccess(string code)
    {
        if (string.IsNullOrEmpty(code))
        {
            return Redirect("/page-not-found");
        }

        return View(new TableBookingDetailModel()
        {
            Code = code.ToUpper()
        });
    }
    
    [Route("booking-table-tracking")]
    [HttpGet]
    public IActionResult BookingTableTracking()
    {
        return View();
    }
    
    [HttpGet]
    public async Task<IActionResult> GetBookingTableTrackingDetail(string code)
    {
        var table = await _tableBookingService.GetDetailByCode(code);

        if (string.IsNullOrEmpty(table.Code))
        {
            return Redirect("/page-not-found");
        }

        return PartialView("_TableBookingTrackingDetail", table);
    }
    
    [Route("table-info")]
    [HttpGet]
    public async Task<IActionResult> TableInfo(string code)
    {
        var table = await _tableService.GetDetailByCode(code);

        if (string.IsNullOrEmpty(table.Code))
        {
            return Redirect("/page-not-found");
        }

        return View(table);
    }
}
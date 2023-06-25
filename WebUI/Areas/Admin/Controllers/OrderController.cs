using Application.Common.Models;
using Application.Interfaces;
using Application.Models.Order;
using Application.Models.Product;
using Application.Models.Table;
using Domain.Constants;
using Domain.Enums;
using Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using WebUI.WebHelper;

namespace WebUI.Areas.Admin.Controllers;

public class OrderController : BaseAdminController
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public IActionResult Index()
    {
        return View();
    }

    public async Task<IActionResult> Detail(string code)
    {
        var order = await _orderService.GetDetail(code);

        if (string.IsNullOrEmpty(order.OrderInfo.Address))
        {
            return Redirect("/page-not-found");
        }

        order.ListDropdownStatus = new ListDropdownStatusOrderModel()
        {
            Id = order.OrderInfo.Id.ToString(),
            CurrentStatus = order.OrderInfo.Status,
            ListStatus = new List<DropdownDetailModel>()
            {
                new DropdownDetailModel()
                {
                    Id = ((int)OrderStatus.AwaitingReview).ToString(),
                    Text = "Chờ Duyệt Đơn"
                },
                new DropdownDetailModel()
                {
                    Id = ((int)OrderStatus.Paid).ToString(),
                    Text = "Đã Thanh Toán"
                },
                new DropdownDetailModel()
                {
                    Id = ((int)OrderStatus.Shipping).ToString(),
                    Text = "Đang Ship"
                },
                new DropdownDetailModel()
                {
                    Id = ((int)OrderStatus.Cancel).ToString(),
                    Text = "Đã Hủy"
                }
            }
        };
        
        return PartialView("_Detail", order);
    }

    [HttpPost]
    public IActionResult UpdateStatus([FromBody] UpdateStatusOrderModel request)
    {
        var dataModal = new ActionWithDialogModel<ListDropdownStatusOrderModel>()
        {
            Data = new ListDropdownStatusOrderModel()
            {
                Id = request.Id,
                CurrentStatus = request.Status,
                ListStatus = new List<DropdownDetailModel>()
                {
                    new DropdownDetailModel()
                    {
                        Id = ((int)OrderStatus.AwaitingReview).ToString(),
                        Text = "Chờ Duyệt Đơn"
                    },
                    new DropdownDetailModel()
                    {
                        Id = ((int)OrderStatus.Paid).ToString(),
                        Text = "Đã Thanh Toán"
                    },
                    new DropdownDetailModel()
                    {
                        Id = ((int)OrderStatus.Shipping).ToString(),
                        Text = "Đang Ship"
                    },
                    new DropdownDetailModel()
                    {
                        Id = ((int)OrderStatus.Cancel).ToString(),
                        Text = "Đã Hủy"
                    }
                }
            },
            Title = "Cập Nhật Trạng Thái Hóa Đơn"
        };

        return PartialView("_UpdateStatus", dataModal);
    }

    [HttpPut]
    public async Task<IActionResult> SubmitUpdateStatus([FromBody] UpdateStatusOrderModel request)
    {
        var result = await _orderService.UpdateStatus(request);

        var msg = result
            ? "Lưu thành công."
            : "Có lỗi sảy ra. Vui lòng thử lại";

        return AppResponse(result, msg, result);
    }

    [HttpGet]
    public async Task<IActionResult> GetListOrder([FromQuery] OrderFilterModel request)
    {
        var orders = await _orderService.GetListOrder(request);
        var paging = PagingHelper<OrderInfoDetailModel>
            .ToPaging(orders, 1, int.MaxValue);

        return AppResponse(true, "Get Success", paging);
    }
}
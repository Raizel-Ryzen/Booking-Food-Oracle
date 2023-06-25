using Application.Common.Models;
using Application.Interfaces;
using Application.Models.Cart;
using Application.Models.Order;
using Domain.Constants;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.WebHelper;

namespace WebUI.Controllers;

public class OrderController : BaseController
{
    private readonly IWebService _webService;
    private readonly IOrderService _orderService;

    public OrderController(IWebService webService, IOrderService orderService)
    {
        _webService = webService;
        _orderService = orderService;
    }

    [HttpGet]
    [Route("order-success")]
    public IActionResult Index(string code)
    {
        if (string.IsNullOrEmpty(code))
        {
            return Redirect("/page-not-found");
        }

        var json = _webService.GetStringSession(SessionKey.Cart);

        if (string.IsNullOrEmpty(json))
        {
            return Redirect("/page-not-found");
        }

        var data = JsonConvert.DeserializeObject<List<AddToCartModel>>(json);

        if (data == null)
        {
            return Redirect("/page-not-found");
        }

        _webService.ClearSession(SessionKey.Cart);

        return View("OrderSuccess", new OrderSuccessModel()
        {
            Code = code,
            ListItems = data
        });
    }

    [HttpGet]
    [Route("order-tracking")]
    public IActionResult OrderTracking()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetOrderTrackingDetail(string code)
    {
        var order = await _orderService.GetDetail(code);

        return PartialView("_OrderTrackingDetail", order);
    }
}
using Application.Interfaces;
using Application.Models.Cart;
using Application.Models.Order;
using Domain.Constants;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebUI.WebHelper;

namespace WebUI.Controllers;

public class CartController : BaseController
{
    private readonly IWebService _webService;
    private readonly IOrderService _orderService;

    public CartController(IWebService webService, IOrderService orderService)
    {
        _webService = webService;
        _orderService = orderService;
    }

    [HttpGet]
    [Route("cart")]
    public IActionResult Index()
    {
        return View();
    }
    
    [HttpGet]
    public IActionResult GetCartList()
    {
        var json = _webService.GetStringSession(SessionKey.Cart);

        if (string.IsNullOrEmpty(json))
        {
            return PartialView("_CartListPartial",new List<AddToCartModel>());
        }

        var data = JsonConvert.DeserializeObject<List<AddToCartModel>>(json);

        return data == null ? PartialView("_CartListPartial",new List<AddToCartModel>()) : PartialView("_CartListPartial",data);
    }

    [HttpPost]
    public IActionResult AddToCart([FromBody] AddToCartModel request)
    {
        var validation = ValidationHelper<AddToCartModel>.IsValid(request);
        if (!validation.IsValid)
        {
            return AppResponse(false, "Vui lòng điền đầy đủ thông tin");
        }

        var currentData = _webService.GetStringSession(SessionKey.Cart);

        if (string.IsNullOrEmpty(currentData))
        {
            var listCart = new List<AddToCartModel>();
            if (request.Quantity > 0)
            {
                listCart.Add(request);
            }
            var json = JsonConvert.SerializeObject(listCart);
            _webService.SetStringSession(SessionKey.Cart, json);
        }
        else
        {
            var listCart = JsonConvert.DeserializeObject<List<AddToCartModel>>(currentData);

            if (listCart != null)
            {
                var existingItem = listCart.FirstOrDefault(x => x.Id == request.Id);
                if (existingItem != null)
                {
                    existingItem.Total = request.Quantity * existingItem.Amount;
                    existingItem.Quantity = request.Quantity;
                }
                else
                {
                    if (request.Quantity > 0)
                    {
                        request.Total = request.Quantity * request.Amount;
                        listCart.Add(request);
                    }
                }

                var json = JsonConvert.SerializeObject(listCart);
                _webService.SetStringSession(SessionKey.Cart, json);
            }
        }

        
        return AppResponse(true, "Đã thêm vào giỏ hàng");
    }
    
    [HttpPost]
    public IActionResult RemoveFromCart([FromBody] RemoveFromCartModel request)
    {
        var validation = ValidationHelper<RemoveFromCartModel>.IsValid(request);
        if (!validation.IsValid)
        {
            return AppResponse(false, "Vui lòng điền đầy đủ thông tin");
        }
        
        var currentData = _webService.GetStringSession(SessionKey.Cart);

        if (string.IsNullOrEmpty(currentData))
        {
            return AppResponse(true, "Đã xóa khỏi giỏ hàng");
        }
        else
        {
            var listCart = JsonConvert.DeserializeObject<List<AddToCartModel>>(currentData);

            if (listCart == null) return AppResponse(true, "Đã xóa khỏi giỏ hàng");
            
            var existingItem = listCart.FirstOrDefault(x => x.Id == request.Id);
            
            if (existingItem != null)
            {
                listCart.Remove(existingItem);
            }

            var json = JsonConvert.SerializeObject(listCart);
            _webService.SetStringSession(SessionKey.Cart, json);
        }

        return AppResponse(true, "Đã xóa khỏi giỏ hàng");
    }

    [Route("checkout")]
    [HttpGet]
    public IActionResult Checkout()
    {
        var json = _webService.GetStringSession(SessionKey.Cart);

        if (string.IsNullOrEmpty(json))
        {
            return View(new List<AddToCartModel>());
        }

        var data = JsonConvert.DeserializeObject<List<AddToCartModel>>(json);

        return data == null ? View(new List<AddToCartModel>()) : View(data);
    }
    
    [HttpPost]
    public async Task<IActionResult> SubmitCheckout([FromBody] CheckoutModel request)
    {
        var validation = ValidationHelper<CheckoutModel>.IsValid(request);
        if (!validation.IsValid)
        {
            return AppResponse(false, "Vui lòng điền đầy đủ thông tin");
        }

        var currentData = _webService.GetStringSession(SessionKey.Cart);

        if (string.IsNullOrEmpty(currentData))
        {
            return AppResponse(false, "Giỏ hàng trống");
        }
        else
        {
            var listCart = JsonConvert.DeserializeObject<List<AddToCartModel>>(currentData);

            if (listCart == null) return AppResponse(false, "Giỏ hàng trống");

            request.Status = (int)OrderStatus.AwaitingReview;
            
            var result = await _orderService.AddNew(new AddNewOrderModel()
            {
                CustomerInfo = request,
                ListCart = listCart
            });
            
            return AppResponse(!string.IsNullOrEmpty(result), !string.IsNullOrEmpty(result) ? "Đã đặt hàng thành công": "Đặt hàng thất bại", result);
        }
    }
}
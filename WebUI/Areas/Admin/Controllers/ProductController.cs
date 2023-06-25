using Application.Common.Models;
using Application.Interfaces;
using Application.Models.Categories;
using Application.Models.Product;
using Domain.Enums;
using Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using WebUI.WebHelper;

namespace WebUI.Areas.Admin.Controllers;

public class ProductController : BaseAdminController
{
    private readonly IProductService _productService;
    private readonly ICommonService _commonService;
    private readonly IGitService _gitService;

    public ProductController(IProductService productService, ICommonService commonService, IGitService gitService)
    {
        _productService = productService;
        _commonService = commonService;
        _gitService = gitService;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    public async Task<IActionResult> AddNewProduct()
    {
        var categories = await _commonService.GetListDropdown(DropdownAction.Category.ReadDescription());
        var units = await _commonService.GetListDropdown(DropdownAction.Unit.ReadDescription());

        var data = new ProductFilterModel()
        {
            Categories = categories,
            Units = units
        };
        
        return View(data);
    }
    
    public async Task<IActionResult> Detail(string id)
    {
        var product = await _productService.Detail(id);
        if (string.IsNullOrEmpty(product.Code))
        {
            return Redirect("/page-not-found");
        }
        var categories = await _commonService.GetListDropdown(DropdownAction.Category.ReadDescription());
        var units = await _commonService.GetListDropdown(DropdownAction.Unit.ReadDescription());

        var data = new ProductFilterModel()
        {
            Categories = categories,
            Units = units,
            Product = product
        };
        return View(data);
    }
    
    [HttpPost]
    public IActionResult UpdateStatus([FromBody] UpdateStatusProductModel request)
    {
        var dataModal = new ActionWithDialogModel<UpdateStatusProductModel>()
        {
            Data = request,
            Title = "Cập Nhật Trạng Thái Món Ăn"
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
        var result = await _productService.Delete(request);
        
        var msg = result
            ? "Lưu thành công."
            : "Có lỗi sảy ra. Vui lòng thử lại";

        return AppResponse(result, msg, result);
    }
    
    [HttpPut]
    public async Task<IActionResult> SubmitUpdateStatus([FromBody] UpdateStatusProductModel request)
    {
        var result = await _productService.UpdateStatus(request);
        
        var msg = result
            ? "Lưu thành công."
            : "Có lỗi sảy ra. Vui lòng thử lại";

        return AppResponse(result, msg, result);
    }
        
    [HttpGet]
    public async Task<IActionResult> GetListProduct([FromQuery] ProductSearchFilterModel request)
    {
        var products = await _productService.GetListProduct(request);
        var paging = PagingHelper<ProductDetailModel>
            .ToPaging(products, 1, int.MaxValue);

        return AppResponse(true, "Get Success", paging);
    }
    
    [HttpPost]
    public async Task<IActionResult> SubmitAddNewProduct([FromForm] AddNewProductModel request)
    {
        var validation = ValidationHelper<AddNewProductModel>.IsValid(request);

        if (!validation.IsValid) return AppResponse(false, "Vui lòng điền đầy đủ thông tin");
        
        if (request.Image?.Length <= 0)
        {
            return AppResponse(false, "Vui lòng điền đầy đủ thông tin");
        }

        if (request.Image != null)
        {
            var image =
                await _gitService.UploadImageAsync(request.Image, GitFolder.BookingFood.ReadDescription());

            if (!image.Success)
            {
                return AppResponse(false, "Không thể Upload hình ảnh");
            }
        
            request.Thumbnail = image.Content?.DownloadUrl ?? string.Empty;
            
            if (string.IsNullOrEmpty(request.Thumbnail))
            {
                return AppResponse(false, "Không thể Upload hình ảnh");
            }
        }
        else
        {
            return AppResponse(false, "Vui lòng điền đầy đủ thông tin");
        }

        var result = await _productService.AddNew(request);

        var msg = result
            ? "Lưu thành công."
            : "Có lỗi sảy ra. Vui lòng thử lại";

        return AppResponse(result, msg, result);
    }
    
    [HttpPut]
    public async Task<IActionResult> SubmitUpdateProduct([FromForm] UpdateProductModel request)
    {
        var validation = ValidationHelper<UpdateProductModel>.IsValid(request);

        if (!validation.IsValid) return AppResponse(false, "Vui lòng điền đầy đủ thông tin");
        
        if (request.Image?.Length <= 0 && string.IsNullOrEmpty(request.CurrentThumbnail))
        {
            return AppResponse(false, "Vui lòng điền đầy đủ thông tin");
        }

        if (request.Image != null)
        {
            var image =
                await _gitService.UploadImageAsync(request.Image, GitFolder.BookingFood.ReadDescription());

            if (!image.Success)
            {
                return AppResponse(false, "Không thể Upload hình ảnh");
            }
        
            request.Thumbnail = image.Content?.DownloadUrl ?? string.Empty;
            
            if (string.IsNullOrEmpty(request.Thumbnail))
            {
                return AppResponse(false, "Không thể Upload hình ảnh");
            }
        }
        else
        {
            request.Thumbnail = request.CurrentThumbnail;
        }
        
        var result = await _productService.Update(request);

        var msg = result
            ? "Lưu thành công."
            : "Có lỗi sảy ra. Vui lòng thử lại";

        return AppResponse(result, msg, result);
    }
}
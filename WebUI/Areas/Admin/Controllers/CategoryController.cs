using Application.Common.Models;
using Application.Interfaces;
using Application.Models.Categories;
using Application.Models.Product;
using Application.Models.Table;
using Domain.Constants;
using Domain.Enums;
using Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
using WebUI.WebHelper;

namespace WebUI.Areas.Admin.Controllers;

public class CategoryController : BaseAdminController
{
    private readonly ICommonService _commonService;
    private readonly ICategoryService _categoryService;

    public CategoryController(ICommonService commonService, ICategoryService categoryService)
    {
        _commonService = commonService;
        _categoryService = categoryService;
    }

    public IActionResult Index()
    {
        return View();
    }
    
    public IActionResult AddNewCategory()
    {
        return View();
    }
    
    public async Task<IActionResult> Detail(string id)
    {
        var data = await _categoryService.GetDetail(id);
        
        if (string.IsNullOrEmpty(data.Title))
        {
            return Redirect("/page-not-found");
        }
        
        return View(data);
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
        var result = await _categoryService.Delete(request);
        
        var msg = result
            ? "Lưu thành công."
            : "Có lỗi sảy ra. Vui lòng thử lại";

        return AppResponse(result, msg, result);
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetListCategory()
    {
        var data = await _categoryService.GetListCategory(new CategoryFilterModel());
        var paging = PagingHelper<CategoryDetailModel>
            .ToPaging(data, 1, int.MaxValue);

        return AppResponse(true, "Get Success", paging);
    }
    
    [HttpPost]
    public async Task<IActionResult> SubmitAddNewCategory([FromBody] AddNewCategoryModel request)
    {
        var validation = ValidationHelper<AddNewCategoryModel>.IsValid(request);

        if (!validation.IsValid) return AppResponse(false, "Vui lòng điền đầy đủ thông tin");

        var result = await _categoryService.AddNew(request);
        

        var msg = result
            ? "Lưu thành công."
            : "Có lỗi sảy ra. Vui lòng thử lại";

        return AppResponse(result, msg, result);
    }
    
    [HttpPut]
    public async Task<IActionResult> SubmitUpdateCategory([FromBody] UpdateCategoryModel request)
    {
        var validation = ValidationHelper<UpdateCategoryModel>.IsValid(request);

        if (!validation.IsValid) return AppResponse(false, "Vui lòng điền đầy đủ thông tin");

        var result = await _categoryService.Update(request);

        var msg = result
            ? "Lưu thành công."
            : "Có lỗi sảy ra. Vui lòng thử lại";

        return AppResponse(result, msg, result);
    }
}
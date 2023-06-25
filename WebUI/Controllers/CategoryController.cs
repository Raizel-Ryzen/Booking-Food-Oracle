using Application.Interfaces;
using Application.Models.Categories;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class CategoryController : BaseController
{
    private readonly ICategoryService _categoryService;

    public CategoryController(ICategoryService categoryService)
    {
        _categoryService = categoryService;
    }

    public IActionResult Index()
    {
        return Redirect("/");
    }
    
    public async Task<IActionResult> GetListCategoryLeftBar([FromQuery] CategoryFilterModel request)
    {
        var data = await _categoryService.GetListCategory(request);
        return PartialView("_CategoryListLeftBarPartial", data);
    }
    
    public async Task<IActionResult> GetListCategoryMenu([FromQuery] CategoryFilterModel request)
    {
        var data = await _categoryService.GetListCategory(request);
        data.Insert(0, new CategoryDetailModel()
        {
            Code = "",
            Title = "Tất Cả"
        });
        return PartialView("_CategoryListMenuPartial", data);
    }
}
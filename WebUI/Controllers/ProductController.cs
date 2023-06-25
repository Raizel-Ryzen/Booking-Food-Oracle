using Application.Interfaces;
using Application.Models.Product;
using Domain.Helpers;
using Microsoft.AspNetCore.Mvc;
namespace WebUI.Controllers;

public class ProductController : BaseController
{
    private readonly IIdentityService _identityService;
    private readonly IProductService _productService;
    private readonly IGitService _gitService;
    private readonly ICommonService _commonService;

    public ProductController(IIdentityService identityService, IProductService productService,
        IGitService gitService, ICommonService commonService)
    {
        _identityService = identityService;
        _productService = productService;
        _gitService = gitService;
        _commonService = commonService;
    }
    
    [HttpGet]
    public async Task<IActionResult> GetListProduct([FromQuery] ProductSearchFilterModel request)
    {
        var products = await _productService.GetListProduct(request);
        var paging = PagingHelper<ProductDetailModel>
            .ToPaging(products, 1, int.MaxValue);

        return PartialView("_ProductListPartial", paging);
    }

    // [Route("deal")]
    // [HttpGet]
    // public async Task<IActionResult> DealDetail(string q, string c)
    // {
    //     if (string.IsNullOrEmpty(q) && string.IsNullOrEmpty(c))
    //     {
    //         return Redirect("/page-not-found");
    //     }
    //
    //     var isUrl = false;
    //     var input = "";
    //
    //     switch (string.IsNullOrEmpty(q))
    //     {
    //         case false when !string.IsNullOrEmpty(c):
    //             isUrl = true;
    //             input = q;
    //             break;
    //         case false when string.IsNullOrEmpty(c):
    //             isUrl = true;
    //             input = q;
    //             break;
    //         default:
    //         {
    //             if (string.IsNullOrEmpty(q) && !string.IsNullOrEmpty(c))
    //             {
    //                 isUrl = false;
    //                 input = c;
    //             }
    //
    //             break;
    //         }
    //     }
    //
    //     var userId = _identityService.GetCurrentUserLogin().Id;
    //     
    //     var product = await _productService.GetDetailDetail(isUrl, input);
    //
    //     if (string.IsNullOrEmpty(product.Code))
    //     {
    //         return Redirect("/page-not-found");
    //     }
    //     
    //     return View(product);
    // }
}
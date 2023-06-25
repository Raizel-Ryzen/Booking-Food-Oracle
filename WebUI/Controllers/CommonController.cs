using Application.Common.Models;
using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class CommonController : BaseController
{
    
    
    [Route("page-not-found")]
    [HttpGet]
    public IActionResult PageNotFound()
    {
        return View();
    }
    
    [Route("no-permission")]
    [HttpGet]
    public IActionResult NoPermission()
    {
        return View();
    }
    
    [Route("error")]
    [HttpGet]
    public IActionResult Error(string msg = "")
    {
        return View(new ErrorModel()
        {
            Code = 400,
            Message = string.IsNullOrEmpty(msg) ? "Có lỗi xảy ra. Vui lòng liên hệ với đội ngũ kỹ thuật của chúng tôi." : msg
        });
    }

    [HttpPost]
    [Authorize(Roles = SecurityRoles.User)]
    public IActionResult CheckLogin()
    {
        return AppResponse(true, "Get Success");
    }
}
using Application.Common.Models;
using Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = SecurityRoles.Admin)]
public class BaseAdminController : Controller
{
    [NonAction]
    protected ActionResult AppResponse(bool isSuccess, string message = "", object? data = null)
    {
        if (isSuccess)
        {
            return new ObjectResult(new SuccessModel()
            {
                Message = message,
                Result = data
            });
        }
        else
        {
            return new BadRequestObjectResult(new ErrorModel()
            {
                Code = 400,
                Message = message
            });
        }
    }
}
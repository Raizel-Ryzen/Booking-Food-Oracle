using Application.Common.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class BaseController : Controller
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
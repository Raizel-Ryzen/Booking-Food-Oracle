using Application.Common.Models;
using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace WebUI.Controllers;

public class AppController : BaseController
{
    private readonly IConfiguration _configuration;
    private readonly IInitDataService _initDataService;
    private readonly IWebService _webService;

    public AppController(IConfiguration configuration, IInitDataService initDataService, IWebService webService)
    {
        _configuration = configuration;
        _initDataService = initDataService;
        _webService = webService;
    }

    [HttpGet]
    public async Task<IActionResult> InsertData()
    {
        //var headerValue = _configuration["AppSetting:AppSecret"];
        
        //if (!_webService.CheckHeaderValue(AppConst.AppSecret, headerValue))
        //    return AppResponse(false,"Invalid Data.");

        await _initDataService.InitDataAsync("AppAll");
        
        return AppResponse(true,"Init data successfully");
    }
}
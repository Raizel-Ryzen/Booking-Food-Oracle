using Application.Common.Models;
using Application.Interfaces;
using Application.Models.Role;
using Application.Models.User;
using Domain.Entities;
using Domain.Helpers;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebUI.Areas.Admin.Controllers;

public class UserController : BaseAdminController
{
    private readonly IUserService _userService;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(
          IUserService userService
        , RoleManager<ApplicationRole> roleManager
        , UserManager<ApplicationUser> userManager)
    {
        _userService = userService;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        return View();
    }

    [HttpGet]
    public async Task<IActionResult> GetListUser()
    {
        var users = await _userService.GetUsers();

        var paging = PagingHelper<UserDetailModel>
            .ToPaging(users, 1, int.MaxValue);
        
        return AppResponse(true, "Get Success", paging);
    }

    [HttpPost]
    public async Task<IActionResult> Delete([FromBody] DeleteModel request)
    {
        var success = await _userService.Delete(request.Id.ToString());

        return Json(false);
    }

    // GET
    public async Task<IActionResult> AddNewUserPartial()
    {
        var data = await _roleManager.Roles.ToListAsync();
        var roles = data.Adapt<List<RoleDetailModel>>();

        return PartialView("_AddNewPartial", roles);
    }

    // GET
    public async Task<IActionResult> DetailPartial(string id)
    {
        var data = await _userService.GetDetail(id);

        return PartialView("_UserDetailPartial", data);
    }

    // GET
    public async Task<IActionResult> UpdateRolePartial(string id)
    {
        var data = await _userService.GetDetail(id);
        var roles = await _roleManager.Roles.ToListAsync();
        data.ListRolesSystem = roles.Adapt<List<RoleDetailModel>>();

        return PartialView("_UpdateRolePartial", data);
    }

    [HttpPost]
    public async Task<IActionResult> SubmitUpdateRole([FromBody] UpdateUserRole request)
    {
        var user = await _userManager.FindByIdAsync(request.Id);

        if (user == null)
        {
            return AppResponse(false, "Không tìm thấy Tài Khoản Người Dùng");
        }

        if (request.Roles?.Count <= 0)
        {
            return AppResponse(false, "Vui lòng chọn ít nhất 1 Quyền Cho Người Dùng");
        }
        
        var userRoles = await _userManager.GetRolesAsync(user);

        await _userManager.RemoveFromRolesAsync(user, userRoles);
        await Task.Delay(500);
        await _userManager.AddToRolesAsync(user, request.Roles);

        return AppResponse(true, "Cập nhật Quyền Người Dùng thành công\nVui lòng nói với người dùng Đăng Nhập lại Website");
    }
    
    public async Task<IActionResult> Detail(string id)
    {
        var data = await _userService.GetDetail(id);
        var roles = await _roleManager.Roles.ToListAsync();
        data.ListRolesSystem = roles.Adapt<List<RoleDetailModel>>();

        return View("UserDetail", data);
    }
}
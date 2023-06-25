using Application.Interfaces;
using Application.Models.Authentication;
using Application.Models.User;
using Domain.Constants;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain.Helpers;
using Mapster;
using WebUI.WebHelper;

namespace WebUI.Controllers;

public class AuthenticateController : BaseController
{
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IIdentityService _identityService;

    public AuthenticateController(SignInManager<ApplicationUser> signInManager
        , IConfiguration configuration
        , UserManager<ApplicationUser> userManager
        , IIdentityService identityService)
    {
        _signInManager = signInManager;
        _configuration = configuration;
        _userManager = userManager;
        _identityService = identityService;
    }
    
    [Route("sign-in")]
    [HttpGet]
    public IActionResult Login()
    {
        if (_identityService.IsAuthorized())
        {
            return Redirect("~/");
        }
        return View();
    }
    
    [Route("sign-up")]
    [HttpGet]
    public IActionResult SignUp()
    {
        if (_identityService.IsAuthorized())
        {
            return Redirect("~/");
        }
        return View();
    }

    [Route("logout")]
    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();

        return Redirect("/");
    }

    [HttpPost]
    public async Task<IActionResult> SignUp([FromBody] AddNewUserModel request)
    {
        var validation = ValidationHelper<AddNewUserModel>.IsValid(request);
        if (!validation.IsValid) return AppResponse(false,"Vui lòng điền đầy đủ thông tin");

        if (request.Password.Length < 4)
        {
            return AppResponse(false,"Mật khẩu phải có ít nhất 4 ký tự");
        }
        
        var existEmail = await _userManager.FindByEmailAsync(request.Email);

        if (existEmail != null)
        {
            return AppResponse(false,$"Email {request.Email} đã được liên kết với tài khoản khác");
        }

        var identityUser = request.Adapt<ApplicationUser>();
        identityUser.FullName = request.FullName;
        identityUser.IsActive = true;
        identityUser.IsDeleted = false;
        identityUser.Picture = AppConst.DefaultAvatar;
        identityUser.CreatedAt = DateTime.Now;
        identityUser.CreatedBy = "system";
        identityUser.SocialJson = "[]";
        identityUser.LastLoginJson = "{}";
        identityUser.NickName = Guid.NewGuid().ToString();
        identityUser.UserName = Guid.NewGuid().ToString();

        var result = await _userManager.CreateAsync(identityUser, request.Password);

        List<string> defaultRoles = new() { SecurityRoles.User };

        await _userManager.AddToRolesAsync(identityUser, defaultRoles);

        var token = TokenHelper.GenerateToken(
            _configuration["JWT:Secret"]
            , _configuration["JWT:ValidIssuer"]
            , _configuration["JWT:ValidAudience"]
            , defaultRoles
            , identityUser);

        HttpContext.Session.SetString(AppConst.SessionJwtKey, token);

        return AppResponse(true,"Đăng ký thành công");
    }

    [HttpPost]
    public async Task<IActionResult> SignIn([FromBody] LoginModel request)
    {
        var validation = ValidationHelper<LoginModel>.IsValid(request);
        if (!validation.IsValid) return AppResponse(false,"Vui lòng điền đầy đủ thông tin");

        var user = await _userManager.FindByEmailAsync(request.Email);;
        
        if (user == null || !await _userManager.CheckPasswordAsync(user, request.Password))
        {
            return AppResponse(false,"Tài khoản hoặc mật khẩu không đúng");
        }

        if (user.IsDeleted)
        {
            return AppResponse(false,"Tài khoản đã bị xóa");
        }

        if (!user.IsActive)
        {
            return AppResponse(false,"Tài khoản đã bị khóa");
        }

        IList<string>? userRoles = await _userManager.GetRolesAsync(user);
        //var selectClaims = await _userManager.GetClaimsAsync(user);

        var token = TokenHelper.GenerateToken(
            _configuration["JWT:Secret"]
            , _configuration["JWT:ValidIssuer"]
            , _configuration["JWT:ValidAudience"]
            , userRoles
            , user);

        HttpContext.Session.SetString(AppConst.SessionJwtKey, token);
        
        return AppResponse(true,"Đăng nhập thành công");
    }

    [HttpPost]
    [AllowAnonymous]
    public IActionResult ExternalLogin(string provider, string returnUrl = null)
    {
        var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Authenticate", new { returnUrl });
        var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
        return Challenge(properties, provider);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null)
    {
        var userExternal = await _signInManager.GetExternalLoginInfoAsync();
        var toNewUser = ExternalLoginHelper.ToNewUser(userExternal);
        var provider = userExternal.ProviderDisplayName;
        var userExists = await _userManager.FindByEmailAsync(toNewUser.Email);
        ApplicationUser user;

        if (userExists != null)
        {
            user = userExists;
        }
        else
        {
            await _userManager.CreateAsync(toNewUser, StringHelper.PasswordGenerate());

            var newUser = await _userManager.FindByEmailAsync(toNewUser.Email);
            List<string> defaultRoles = new() { SecurityRoles.User };

            await _userManager.AddToRolesAsync(newUser, defaultRoles);

            user = newUser;
        }

        if (user.IsDeleted)
        {
            return Redirect("~/");
        }

        IList<string>? userRoles = await _userManager.GetRolesAsync(user);
        //var selectClaims = await _userManager.GetClaimsAsync(user);

        var token = TokenHelper.GenerateToken(
            _configuration["JWT:Secret"]
            , _configuration["JWT:ValidIssuer"]
            , _configuration["JWT:ValidAudience"]
            , userRoles
            , user);

        HttpContext.Session.SetString(AppConst.SessionJwtKey, token);

        return Redirect("~/");
    }
    
}
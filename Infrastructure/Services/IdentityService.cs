using System.Linq;
using System.Security.Claims;
using Application.Common.Models;
using Application.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Http;

namespace Infrastructure.Services;

public class IdentityService : IIdentityService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IdentityService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public CurrentUserModel GetCurrentUserLogin()
    {
        var user = _httpContextAccessor.HttpContext?.User;
        var userId = "";
        var nameIdentifier =
            user?.Claims.FirstOrDefault(s => s.Type.Contains(AppJwtClaimTypes.NameIdentifier))?.Value;
        var sub = user?.Claims.FirstOrDefault(s => s.Type.Contains(AppJwtClaimTypes.NameIdentifier))?.Value;

        if (!string.IsNullOrEmpty(sub))
        {
            userId = sub;
        }
        else if (!string.IsNullOrEmpty(nameIdentifier))
        {
            userId = nameIdentifier;
        }

        return new CurrentUserModel
        {
            UserName = user?.Claims.FirstOrDefault(s => s.Type.Contains(AppJwtClaimTypes.UserName))?.Value,
            EmailConfirmed =
                user?.Claims.FirstOrDefault(s => s.Type.Contains(AppJwtClaimTypes.ConfirmedEmail))?.Value == "true",
            FullName = user?.Claims.FirstOrDefault(s => s.Type.Contains(AppJwtClaimTypes.FullName))?.Value,
            Id = (string.IsNullOrEmpty(userId) ? "" : userId?.ToLower()) ?? "",
            NickName = user?.Claims.FirstOrDefault(s => s.Type.Contains(AppJwtClaimTypes.NickName))?.Value,
            DateOfBirth = user?.Claims.FirstOrDefault(s => s.Type.Contains(AppJwtClaimTypes.DateOfBirth))?.Value,
            Picture = user?.Claims.FirstOrDefault(s => s.Type.Contains(AppJwtClaimTypes.Picture))?.Value,
            Email = user?.Claims.FirstOrDefault(s => s.Type.Contains(AppJwtClaimTypes.Email))?.Value,
        };
    }

    public bool IsAuthorized()
    {
        return !string.IsNullOrEmpty(GetCurrentUserLogin().Id);
    }

    public bool IsRole(string role)
    {
        var isRole = _httpContextAccessor.HttpContext?.User?.IsInRole(role);
        return isRole ?? false;
    }

    public bool IsEmailConfirmed()
    {
        return GetCurrentUserLogin().EmailConfirmed ?? false;
    }
}
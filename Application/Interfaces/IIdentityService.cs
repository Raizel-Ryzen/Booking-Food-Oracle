using Application.Common.Models;

namespace Application.Interfaces;

public interface IIdentityService
{
    CurrentUserModel GetCurrentUserLogin();
    bool IsAuthorized();
    bool IsRole(string role);
    bool IsEmailConfirmed();
}
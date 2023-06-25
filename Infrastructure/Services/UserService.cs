using Application.Interfaces;
using Application.Models.User;
using Domain.Entities;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace Infrastructure.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _configuration;
    private readonly IApplicationDbContext _dbContext;

    public UserService(UserManager<ApplicationUser> userManager, IConfiguration configuration,
        IApplicationDbContext dbContext)
    {
        _userManager = userManager;
        _configuration = configuration;
        _dbContext = dbContext;
    }

    public async Task<List<UserDetailModel>> GetUsers()
    {
        var users = await _userManager.Users
            .Where(s => !s.IsDeleted)
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        var list = from applicationUser in users
            select new
            {
                Id = applicationUser.Id, FullName = applicationUser.FullName, UserName = applicationUser.UserName,
                Email = applicationUser.Email, PhoneNumber = applicationUser.PhoneNumber,
                Roles = string.Join(", ", _userManager.GetRolesAsync(applicationUser).Result.ToArray()),
                CreatedAt = applicationUser.CreatedAt, Thumbnail = applicationUser.Picture,
                UpdatedAt = applicationUser.UpdatedAt, Status = applicationUser.IsActive ? 2 : 1
            };

        return list.Adapt<List<UserDetailModel>>();
    }

    public async Task<bool> Delete(string id)
    {
        try
        {
            var user = await _userManager.FindByIdAsync(id);
            user.IsDeleted = true;
            await _userManager.UpdateAsync(user);

            return true;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return false;
        }
    }

    public async Task<UserDetailModel> GetDetail(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user == null)
        {
            return new UserDetailModel();
        }

        var data = user.Adapt<UserDetailModel>();
        var roles = await _userManager.GetRolesAsync(user);
        data.ListRole = roles;
        data.Roles = string.Join(", ", roles.ToArray());

        return data;
    }
}
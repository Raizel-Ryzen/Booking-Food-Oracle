using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Interfaces;
using Application.Models.Categories;
using Application.Models.Unit;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using Domain.Helpers;
using Mapster;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services;

public class InitDataService : IInitDataService
{
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;

    public InitDataService(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        IApplicationDbContext dbContext,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _dbContext = dbContext;
        _configuration = configuration;
    }

    public async Task InitDataAsync(string action)
    {
        if (action.Equals(InitDataAction.AppAll.ReadDescription()))
        {
            await InitRoles();
            await InitUsers();
            await InitCategories();
            await InitUnits();
        }
        else
        {
            if (action.Equals(InitDataAction.AppRole.ReadDescription()))
            {
                await InitRoles();
            }
            else if (action.Equals(InitDataAction.AppUser.ReadDescription()))
            {
                await InitUsers();
            }
            else if (action.Equals(InitDataAction.AppCategory.ReadDescription()))
            {
                await InitCategories();
            }
            else if (action.Equals(InitDataAction.AppUnit.ReadDescription()))
            {
                await InitUnits();
            }
        }
    }

    private async Task InitRoles()
    {
        var index = 0;
        foreach (var role in SecurityRoles.Roles)
        {
            var guidId = $"F45695BB-F459-4CCC-83F5-72BFCAB3DBC{index}";

            if (!await _roleManager.RoleExistsAsync(role))
            {
                await _roleManager.CreateAsync(new ApplicationRole
                {
                    Id = Guid.Parse(guidId),
                    Name = role,
                    NormalizedName = role.ToUpper(),
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    Description = $"Role for {role}"
                });
                index++;
            }
        }
    }

    private async Task InitUsers()
    {
        #region data for init

        var currentDate = DateTime.UtcNow;
        List<UserInitModel> data = new()
        {
            // full role
            new UserInitModel
            {
                Id = Guid.Parse("F45695BB-F459-4CCC-83F5-72BFCAB3DBC0"),
                UserName = "super_admin",
                Email = "super_admin@gmail.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                Roles = SecurityRoles.Roles.ToList(),
                FullName = "Super Admin",
                IsActive = true,
                IsDeleted = false,
                CreatedBy = AppConst.AppDomain,
                SocialJson = "[]",
                LastLoginJson = "{}",
                Picture = AppConst.DefaultAvatar,
                NickName = Guid.NewGuid().ToString()
            },
            // user
            new UserInitModel
            {
                Id = Guid.Parse("F45695BB-F459-4CCC-83F5-72BFCAB3DBC1"),
                UserName = "user",
                Email = "user@gmail.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                Roles = SecurityRoles.Roles.Where(s => !s.Equals(SecurityRoles.Admin)
                                                       && !s.Equals(SecurityRoles.Manager)
                                                       && !s.Equals(SecurityRoles.Seller)).ToList(),
                FullName = "User",
                IsActive = true,
                IsDeleted = false,
                CreatedBy = AppConst.AppDomain,
                SocialJson = "[]",
                LastLoginJson = "{}",
                Picture = AppConst.DefaultAvatar,
                NickName = Guid.NewGuid().ToString()
            },
            // seller
            new UserInitModel
            {
                Id = Guid.Parse("F45695BB-F459-4CCC-83F5-72BFCAB3DBC2"),
                UserName = "seller",
                Email = "seller@gmail.com",
                SecurityStamp = Guid.NewGuid().ToString(),
                Roles = SecurityRoles.Roles.Where(s => !s.Equals(SecurityRoles.Admin)
                                                       && !s.Equals(SecurityRoles.Manager)).ToList(),
                FullName = "Seller",
                IsActive = true,
                IsDeleted = false,
                CreatedBy = AppConst.AppDomain,
                SocialJson = "[]",
                LastLoginJson = "{}",
                Picture = AppConst.DefaultAvatar,
                NickName = Guid.NewGuid().ToString()
            }
        };

        #endregion

        foreach (var item in data)
        {
            var user = await _userManager.FindByEmailAsync(item.Email);

            if (user == null)
            {
                await _userManager.CreateAsync(item.Adapt<ApplicationUser>(),
                    _configuration["AppSetting:DefaultPassword"]);
            }
            else
            {
                await _userManager.RemoveFromRolesAsync(user, SecurityRoles.Roles);
                await _userManager.DeleteAsync(user);
                await _userManager.CreateAsync(item.Adapt<ApplicationUser>(),
                    _configuration["AppSetting:DefaultPassword"]);
            }

            var userExist = await _userManager.FindByEmailAsync(item.Email);

            if (userExist == null)
            {
                continue;
            }

            await _userManager.RemoveFromRolesAsync(userExist, SecurityRoles.Roles);
            await _userManager.AddToRolesAsync(userExist, item.Roles);
        }
    }

    private async Task InitCategories()
    {
        #region data for categories

        var data = new List<CategoryDetailModel>()
        {
            new CategoryDetailModel()
            {
                Id = Guid.Parse("F15095BB-F459-4CCC-83F5-72BFCAB3DBC0"),
                Code = "mon-khai-vi",
                Title = "Món Khai Vị"
            },
            new CategoryDetailModel()
            {
                Id = Guid.Parse("F15095BB-F459-4CCC-83F5-72BFCAB3DBC1"),
                Code = "do-hai-san",
                Title = "Đồ Hải Sản"
            },
            new CategoryDetailModel()
            {
                Id = Guid.Parse("F15095BB-F459-4CCC-83F5-72BFCAB3DBC3"),
                Code = "mon-lau",
                Title = "Món Lẩu"
            },
            new CategoryDetailModel()
            {
                Id = Guid.Parse("F15095BB-F459-4CCC-83F5-72BFCAB3DBC4"),
                Code = "mon-nuong",
                Title = "Món Nướng"
            },
            new CategoryDetailModel()
            {
                Id = Guid.Parse("F15095BB-F459-4CCC-83F5-72BFCAB3DBC2"),
                Code = "mon-rau",
                Title = "Món Rau"
            },
            new CategoryDetailModel()
            {
                Id = Guid.Parse("F15095BB-F459-4CCC-83F5-72BFCAB3DBC5"),
                Code = "mon-trang-mieng",
                Title = "Món Tráng Miệng"
            },
            new CategoryDetailModel()
            {
                Id = Guid.Parse("F15095BB-F459-4CCC-83F5-72BFCAB3DBC6"),
                Code = "nuoc-ngot",
                Title = "Nước Ngọt"
            },
        };

        #endregion

        var dataAdapt = data.Adapt<List<Category>>();
        var dataFinal = new List<Category>();

        foreach (var item in dataAdapt)
        {
            var exists = await _dbContext.Categories.FirstOrDefaultAsync(s => s.Title == item.Title);

            if (exists == null)
            {
                dataFinal.Add(item);
            }
        }

        if (dataFinal?.Count > 0)
        {
            await _dbContext.Categories.AddRangeAsync(dataFinal);
            var result = await _dbContext.SaveChangesAsync(new CancellationToken());
        }
    }

    private async Task InitUnits()
    {
        #region data for units

        var data = new List<UnitDetailModel>()
        {
            new UnitDetailModel()
            {
                Id = Guid.Parse("F15095BB-F459-4CCC-83F5-72BFCAB3DBC0"),
                Code = "1-dia",
                Title = "1 Dĩa"
            },
            new UnitDetailModel()
            {
                Id = Guid.Parse("F15095BB-F459-4CCC-83F5-72BFCAB3DBC1"),
                Code = "1-lit",
                Title = "1 Lít"
            },
            new UnitDetailModel()
            {
                Id = Guid.Parse("F15095BB-F459-4CCC-83F5-72BFCAB3DBC3"),
                Code = "1-noi-size-nho",
                Title = "1 Nồi Size Nhỏ"
            },
            new UnitDetailModel()
            {
                Id = Guid.Parse("F15095BB-F459-4CCC-83F5-72BFCAB3DBC4"),
                Code = "1-noi-size-vua",
                Title = "1 Nồi Size Vừa"
            },
            new UnitDetailModel()
            {
                Id = Guid.Parse("F15095BB-F459-4CCC-83F5-72BFCAB3DBC5"),
                Code = "1-noi-size-vua",
                Title = "1 Nồi Size Lớn"
            }
        };

        #endregion

        var dataAdapt = data.Adapt<List<Unit>>();
        var dataFinal = new List<Unit>();

        foreach (var item in dataAdapt)
        {
            var exists = await _dbContext.Units.FirstOrDefaultAsync(s => s.Title == item.Title);

            if (exists == null)
            {
                dataFinal.Add(item);
            }
        }

        if (dataFinal?.Count > 0)
        {
            await _dbContext.Units.AddRangeAsync(dataFinal);
            var result = await _dbContext.SaveChangesAsync(new CancellationToken());
        }
    }
}
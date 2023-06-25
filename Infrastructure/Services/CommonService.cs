using Application.Common.Models;
using Application.Interfaces;
using Application.Models.Categories;
using Application.Models.Home;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class CommonService : ICommonService
{
    private readonly IApplicationDbContext _dbContext;

    public CommonService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<DashboardAdminModel> GetDashboardAdmin()
    {
        return new DashboardAdminModel()
        {
            TotalDealApproved = 1,
            TotalShopApproved = 1,
            TotalDealAwaitingApproval = 1,
            TotalShopAwaitingApproval = 1,
        };
    }

   public async Task<List<DropdownDetailModel>> GetListDropdown(string action)
    {
        var listDropdown = new List<DropdownDetailModel>();
        
        switch (action)
        {
            case "Category":
                var categories = await _dbContext.Categories.Where(s => !s.IsDeleted).ToListAsync();
                var newCate = from item in categories
                    select new
                    {
                        Id = item.Id.ToString(),
                        Text = item.Title
                    };
                listDropdown = newCate.Adapt<List<DropdownDetailModel>>();
                break;
            case "Unit":
                var cities = await _dbContext.Units.Where(s => !s.IsDeleted).ToListAsync();
                var newCities= from item in cities
                    select new
                    {
                        Id = item.Id.ToString(),
                        Text = item.Title
                    };
                listDropdown = newCities.Adapt<List<DropdownDetailModel>>();
                break;
            default:
                return listDropdown;
        }

        return listDropdown;
    }
}
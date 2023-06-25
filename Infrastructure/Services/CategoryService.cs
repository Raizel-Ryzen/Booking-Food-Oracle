using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Models;
using Application.Interfaces;
using Application.Models.Categories;
using Domain.Entities;
using Domain.Helpers;
using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class CategoryService : ICategoryService
{
    private readonly IApplicationDbContext _dbContext;

    public CategoryService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<CategoryDetailModel>> GetListCategory(CategoryFilterModel model)
    {
        var data = await _dbContext.Categories
            .Where(s => !s.IsDeleted 
                        && (string.IsNullOrEmpty(model.Title) || s.Title.ToLower().Equals(model.Title.ToLower())))
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return data.Adapt<List<CategoryDetailModel>>();
    }

    public async Task<CategoryDetailModel> GetDetail(string id)
    {
        var data = await _dbContext.Categories
            .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == Guid.Parse(id));
        
        return data == null ? new CategoryDetailModel() : data.Adapt<CategoryDetailModel>();
    }

    public async Task<bool> Delete(DeleteModel model)
    {
        var data = await _dbContext.Categories
            .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == Guid.Parse(model.Id));

        if (data == null)
        {
            return false;
        }

        data.IsDeleted = true;

        _dbContext.Categories.Update(data);
        var result = await _dbContext.SaveChangesAsync(new CancellationToken());

        return result > 0;
    }

    public async Task<bool> AddNew(AddNewCategoryModel model)
    {
        var newData = model.Adapt<Category>();
        newData.Code = model.Title.ToStringCode();

        await _dbContext.Categories.AddAsync(newData);

        var result = await _dbContext.SaveChangesAsync(new CancellationToken());

        return result > 0;
    }

    public async Task<bool> Update(UpdateCategoryModel model)
    {
        var data = await _dbContext.Categories
            .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == model.Id);

        if (data == null)
        {
            return false;
        }

        data.Title = model.Name;
        data.Code = model.Name.ToStringCode();

        _dbContext.Categories.Update(data);

        var result = await _dbContext.SaveChangesAsync(new CancellationToken());
 
        return result > 0;
    }
}
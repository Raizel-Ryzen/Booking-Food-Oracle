using Application.Common.Models;
using Application.Interfaces;
using Application.Models.Product;
using Domain.Entities;
using Domain.Enums;
using Domain.Helpers;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Services;

public class ProductService : IProductService
{
    private readonly IApplicationDbContext _dbContext;

    public ProductService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> AddNew(AddNewProductModel model)
    {
        var id = Guid.NewGuid();
        var code = id.ToString().Split('-').First();
        var product = model.Adapt<Product>();

        product.Id = id;
        product.Code = code;
        product.Status = (int)ProductStatus.Available;
        product.Url = model.Title.UniqueUrlWithBaseCode(code);
        product.Code = code;
        product.Bought = 0;

        await _dbContext.Products.AddAsync(product);
        var saveProduct = await _dbContext.SaveChangesAsync(new CancellationToken());

        return saveProduct > 0;
    }

    public async Task<bool> Update(UpdateProductModel model)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(s => s.Id == Guid.Parse(model.Id) && !s.IsDeleted);
       
        if (product==null)
        {
            return false;
        }
        
        product.Title = model.Title;
        product.Amount = model.Amount;
        product.Thumbnail = model.Thumbnail;
        product.CategoryId = Guid.Parse(model.CategoryId);
        product.UnitId = Guid.Parse(model.UnitId);

        _dbContext.Products.Update(product);
        
        var saveChangesAsync = await _dbContext.SaveChangesAsync(new CancellationToken());
        
        return saveChangesAsync > 0;
    }

    public async Task<bool> Delete(DeleteModel model)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(s => s.Id == Guid.Parse(model.Id) && !s.IsDeleted);
       
        if (product==null)
        {
            return false;
        }

        product.IsDeleted = true;

        _dbContext.Products.Update(product);
        
        var saveChangesAsync = await _dbContext.SaveChangesAsync(new CancellationToken());
        
        return saveChangesAsync > 0;
    }

    public async Task<ProductDetailModel> Detail(string id)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(s => s.Id == Guid.Parse(id) && !s.IsDeleted);
       
        return product==null ? new ProductDetailModel() : product.Adapt<ProductDetailModel>();
    }

    public async Task<List<ProductDetailModel>> GetListProduct(ProductSearchFilterModel request)
    {
        var products = await _dbContext.Products.Where(s =>
                !s.IsDeleted
                && (request.Status == null || request.Status == s.Status || request.Status == 0)
                && (string.IsNullOrEmpty(request.CategoryId) || Guid.Parse(request.CategoryId) == s.CategoryId)
                && (string.IsNullOrEmpty(request.SearchText) ||
                    s.Title.ToLower().Contains(request.SearchText.ToLower()))
            )
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();

        var units = await _dbContext.Units.Where(s => !s.IsDeleted).ToListAsync();

        var categories = await _dbContext.Categories.Where(s => !s.IsDeleted).ToListAsync();
        
        var productJoin = from product in products
            join unit in units
                on product.UnitId equals unit.Id
            join category in categories
                on product.UnitId equals category.Id
            select new
            {
                Id = product.Id,
                Title = product.Title,
                Code = product.Code,
                Thumbnail = product.Thumbnail,
                Amount = product.Amount,
                Status = product.Status,
                Bought = product.Bought,
                Url = product.Url,
                CategoryId = product.CategoryId,
                UnitId = product.UnitId,
                CategoryName = category.Title,
                UnitName = unit.Title,
            };

        var dataReturn = productJoin.Adapt<List<ProductDetailModel>>();

        if (!(request?.IsOrderByOnly > 0)) return dataReturn;

        var orderBy = request.OrderBy + request.OrderType;

        if (ProductOrderBy.BoughtDesc.ReadDescription().Equals(orderBy))
        {
            dataReturn = dataReturn.OrderByDescending(s => s.Bought).ToList();
        }
        else if (ProductOrderBy.AmountAsc.ReadDescription().Equals(orderBy))
        {
            dataReturn = dataReturn.OrderBy(s => s.Amount).ToList();
        }
        else if (ProductOrderBy.AmountDesc.ReadDescription().Equals(orderBy))
        {
            dataReturn = dataReturn.OrderByDescending(s => s.Amount).ToList();
        }

        return dataReturn;
    }

    public async Task<bool> UpdateStatus(UpdateStatusProductModel model)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(s => s.Id == Guid.Parse(model.Id) && !s.IsDeleted);
       
        if (product==null)
        {
            return false;
        }

        product.Status = model.Status;

        _dbContext.Products.Update(product);
        
        var saveChangesAsync = await _dbContext.SaveChangesAsync(new CancellationToken());
        
        return saveChangesAsync > 0;
    }
}
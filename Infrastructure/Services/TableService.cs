using Application.Common.Models;
using Application.Interfaces;
using Application.Models.Categories;
using Application.Models.Table;
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

public class TableService : ITableService
{
    private readonly IApplicationDbContext _dbContext;

    public TableService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TableDetailModel>> GetListTable(TableFilterModel model)
    {
        var data = await _dbContext.Tables
            .Where(s => !s.IsDeleted
                        && (model.Status == 0 || model.Status == null || model.Status == s.Status)
                        && (string.IsNullOrEmpty(model.Title) || s.Title.ToLower().Equals(model.Title.ToLower())))
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();

        return data.Adapt<List<TableDetailModel>>();
    }

    public async Task<TableDetailModel> GetDetail(string id)
    {
        var data = await _dbContext.Tables
            .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == Guid.Parse(id));

        return data == null ? new TableDetailModel() : data.Adapt<TableDetailModel>();
    }

    public async Task<bool> Delete(DeleteModel model)
    {
        var data = await _dbContext.Tables
            .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == Guid.Parse(model.Id));

        if (data == null)
        {
            return false;
        }

        data.IsDeleted = true;

        _dbContext.Tables.Update(data);
        var result = await _dbContext.SaveChangesAsync(new CancellationToken());

        return result > 0;
    }

    public async Task<bool> AddNew(AddNewTableModel model)
    {
        var newData = model.Adapt<Table>();

        newData.Status = (int)TableStatus.Available;
        
        await _dbContext.Tables.AddAsync(newData);

        var result = await _dbContext.SaveChangesAsync(new CancellationToken());

        return result > 0;
    }

    public async Task<bool> Update(UpdateTableModel model)
    {
        var data = await _dbContext.Tables
            .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == model.Id);

        if (data == null)
        {
            return false;
        }

        data.Title = model.Title;
        data.SlotNumber = model.SlotNumber;
        
        _dbContext.Tables.Update(data);

        var result = await _dbContext.SaveChangesAsync(new CancellationToken());

        return result > 0;
    }

    public async Task<bool> UpdateStatus(UpdateStatusTableModel model)
    {
        var data = await _dbContext.Tables
            .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == Guid.Parse(model.Id));

        if (data == null)
        {
            return false;
        }

        data.Status = model.Status;

        _dbContext.Tables.Update(data);

        var result = await _dbContext.SaveChangesAsync(new CancellationToken());

        return result > 0;
    }

    public async Task<bool> UpdateSessionId(UpdateTableSessionIdModel model)
    {
        var data = await _dbContext.Tables
            .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == Guid.Parse(model.Id));

        if (data == null)
        {
            return false;
        }

        data.SessionId = Guid.Parse(model.SessionId);

        _dbContext.Tables.Update(data);

        var result = await _dbContext.SaveChangesAsync(new CancellationToken());

        return result > 0;
    }

    public async Task<TableDetailModel> GetDetailByCode(string code)
    {
        var data = await _dbContext.Tables
            .FirstOrDefaultAsync(s => !s.IsDeleted && s.Code == code);

        return data == null ? new TableDetailModel() : data.Adapt<TableDetailModel>();
    }
}
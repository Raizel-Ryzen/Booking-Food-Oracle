using Application.Interfaces;
using Application.Models.Table;
using Application.Models.TableBooking;
using Domain.Constants;
using Domain.Entities;
using Domain.Enums;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace Infrastructure.Services;

public class TableBookingService : ITableBookingService
{
    private readonly IApplicationDbContext _dbContext;

    public TableBookingService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<TableBookingDetailModel>> GetListTableBooking(TableFilterModel model)
    {
        var tableBookings = await _dbContext.TableBookings
            .Where(s => !s.IsDeleted && (model.Status == 0 || model.Status == null || model.Status == s.Status))
            .OrderByDescending(d => d.CreatedAt)
            .ToListAsync();
        
        var tableIds = tableBookings.Select(s => s.TableId).ToList();
        var tables = await _dbContext.Tables.Where(s => !s.IsDeleted && tableIds.Contains(s.Id)).ToListAsync();

        var data = from tableBooking in tableBookings
            join table in tables on tableBooking.TableId equals table.Id
            select new
            {
                Id = tableBooking.Id,
                Code = tableBooking.Code,
                CustomerName = tableBooking.CustomerName,
                Address = tableBooking.Address,
                PhoneNumber = tableBooking.PhoneNumber,
                TableId = table.Id,
                TableStatus = table.Status,
                Status = tableBooking.Status,
                IntendTime = tableBooking.IntendTime,
                ReceivedDate = tableBooking.ReceivedDate,
                TimeInfo = tableBooking.TimeInfo,
                SessionId = tableBooking.SessionId,
                TableName = table.Title,
                QRCode = table.QRCode,
                IsEdit = tableBooking.IsEdit
            };

        return data.Adapt<List<TableBookingDetailModel>>();
    }

    public async Task<TableBookingDetailModel> GetDetail(string id)
    {
        var tableBooking =
            await _dbContext.TableBookings.FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == Guid.Parse(id));

        if (tableBooking == null)
        {
            return new TableBookingDetailModel();
        }

        var table = await _dbContext.Tables.FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == tableBooking.TableId);

        if (table == null)
        {
            return new TableBookingDetailModel();
        }

        var data = tableBooking.Adapt<TableBookingDetailModel>();
        data.TableStatus = table.Status;
        data.TableName = table.Title;

        return data;
    }

    public async Task<bool> UpdateStatus(UpdateStatusTableBookingModel model)
    {
        var data = await _dbContext.TableBookings
            .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == Guid.Parse(model.Id));
        
        if (data == null)
        {
            return false;
        }

        var table = await _dbContext.Tables
            .FirstAsync(s => !s.IsDeleted && s.Id == data.TableId);

        if (data.IsEdit)
        {
            return false;
        }

        switch (model.Status)
        {
            case (int)TableBookingStatus.Approved:
                data.Status = (int)TableBookingStatus.Approved;
                table.Status = (int)TableStatus.NotAvailable;
                break;
            case (int)TableBookingStatus.Reject:
                data.Status = (int)TableBookingStatus.Reject;
                table.Status = (int)TableStatus.Available;
                break;
        }

        _dbContext.Tables.Update(table);
        await _dbContext.SaveChangesAsync(new CancellationToken());

        data.IsEdit = true;
        _dbContext.TableBookings.Update(data);
        var result = await _dbContext.SaveChangesAsync(new CancellationToken());

        return result > 0;
    }

    public async Task<string> AddNew(AddNewTableBookingModel model)
    {
        var newData = model.Adapt<TableBooking>();
        newData.Code = Guid.NewGuid().ToString().Split('-').First().ToUpper();
        newData.RestaurantAddress = AppConst.RestaurantAddress;
        newData.Status = (int)TableBookingStatus.AwaitingReview;
        newData.IsEdit = false;
        
        await _dbContext.TableBookings.AddAsync(newData);

        var result = await _dbContext.SaveChangesAsync(new CancellationToken());

        if (result <= 0)
        {
            return "";
        }

        var data = await _dbContext.Tables
            .FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == Guid.Parse(model.TableId));

        if (data == null)
        {
            return "";
        }

        data.Status = (int)TableStatus.NotAvailable;

        _dbContext.Tables.Update(data);

        var result2 = await _dbContext.SaveChangesAsync(new CancellationToken());

        return result2 <= 0 ? "" : newData.Code;
    }

    public async Task<TableBookingDetailModel> GetDetailByCode(string code)
    {
        var tableBooking = await _dbContext.TableBookings.FirstOrDefaultAsync(s => !s.IsDeleted && s.Code == code);

        if (tableBooking == null)
        {
            return new TableBookingDetailModel();
        }

        var table = await _dbContext.Tables.FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == tableBooking.TableId);

        if (table == null)
        {
            return new TableBookingDetailModel();
        }

        var data = tableBooking.Adapt<TableBookingDetailModel>();
        data.TableStatus = table.Status;
        data.TableName = table.Title;

        return data;
    }
}
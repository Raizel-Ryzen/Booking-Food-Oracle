using Application.Common.Models;
using Application.Interfaces;
using Application.Models.Order;
using Domain.Entities;
using Mapster;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
namespace Infrastructure.Services;

public class OrderService : IOrderService
{
    private readonly IApplicationDbContext _dbContext;

    public OrderService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<OrderInfoDetailModel>> GetListOrder(OrderFilterModel model)
    {
        var data = await _dbContext.Orders
            .Where(s => !s.IsDeleted
                        && (model.Status == 0 || model.Status == null || model.Status == s.Status))
            .OrderByDescending(a => a.CreatedAt)
            .ToListAsync();
        
        return data.Adapt<List<OrderInfoDetailModel>>();
    }

    public async Task<OrderDetailModel> GetDetail(string code)
    {
        var orderDetail = await _dbContext.Orders.FirstOrDefaultAsync(s => !s.IsDeleted && s.Code.Equals(code));

        if (orderDetail == null)
        {
            return new OrderDetailModel()
            {
                OrderInfo = new OrderInfoDetailModel()
                {
                    Code = ""
                }
            };
        }

        var listItems = await _dbContext.OrderItems.Where(s => !s.IsDeleted && s.OrderId == orderDetail.Id)
            .ToListAsync();

        var productIds = listItems.Select(s => s.ProductId);

        var products = await _dbContext.Products.Where(s => productIds.Contains(s.Id)).ToListAsync();

        var dataItems = from product in products
            join orderItem in listItems
                on product.Id equals orderItem.ProductId
            select new
            {
                ProductName = product.Title,
                Quantity = orderItem.Quantity,
                Amount = product.Amount,
                TotalAmount = orderItem.TotalAmount,
                IsDeleted = product.IsDeleted
            };

        var productOrderItem = dataItems.Adapt<List<OrderItemDetailModel>>();
        var orderInfo = orderDetail.Adapt<OrderInfoDetailModel>();
        
        return new OrderDetailModel()
        {
            OrderInfo = orderInfo,
            ListItems = productOrderItem
        };
    }

    public async Task<bool> Delete(DeleteModel model)
    {
        throw new NotImplementedException();
    }

    public async Task<string> AddNew(AddNewOrderModel model)
    {
        var id = Guid.NewGuid();
        var order = model.CustomerInfo.Adapt<Order>();
        order.Code = id.ToString().Split('-').First().ToUpper();
        order.TotalAmount = model.ListCart.Sum(item => item.Amount * item.Quantity);
        order.Id = id;

        await _dbContext.Orders.AddAsync(order);
        var result1 = await _dbContext.SaveChangesAsync(new CancellationToken());

        if (result1 <= 0)
        {
            return "";
        }

        var orderItems = model.ListCart.Select(item => new OrderItem()
        {
            Quantity = item.Quantity, OrderId = id, ProductId = Guid.Parse(item.Id),
            TotalAmount = item.Quantity * item.Amount
        }).ToList();

        await _dbContext.OrderItems.AddRangeAsync(orderItems);
        var result2 = await _dbContext.SaveChangesAsync(new CancellationToken());

        return result2 > 0 ? order.Code : "";
    }

    public async Task<bool> UpdateStatus(UpdateStatusOrderModel model)
    {
        var orderDetail = await _dbContext.Orders.FirstOrDefaultAsync(s => !s.IsDeleted && s.Id == Guid.Parse(model.Id));

        if (orderDetail == null)
        {
            return false;
        }

        orderDetail.Status = model.Status;
        
        _dbContext.Orders.Update(orderDetail);
        var result = await _dbContext.SaveChangesAsync(new CancellationToken());
        
        return result > 0;
    }
}
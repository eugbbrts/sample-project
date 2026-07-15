using BusinessEntities;
using System;
using System.Collections.Generic;

namespace Core.Services.Orders
{
    public interface IGetOrderService
    {
        Order GetOrder(Guid id);
        IEnumerable<Order> GetUserOrders(Guid userId);
        IEnumerable<Order> GetOrdersByDateRange(DateTime? createdFrom, DateTime? createdTo);
    }
}

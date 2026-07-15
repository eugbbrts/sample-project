using BusinessEntities;
using System;
using System.Collections.Generic;

namespace Data.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        bool Exists(Guid id);
        IEnumerable<Order> GetUserOrders(Guid userId);
        IEnumerable<Order> GetOrdersByDateRange(DateTime? createdFrom, DateTime? crearedTo);
    }
}

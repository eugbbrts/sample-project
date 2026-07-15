using BusinessEntities;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories
{
    [AutoRegister]
    public class OrderRepository : InMemoryRepository<Order>, IOrderRepository
    {
        public bool Exists(Guid id)
        {
            return _dataStore.ContainsKey(id);
        }

        public IEnumerable<Order> GetUserOrders(Guid userId)
        {
            return _dataStore.Values.Where(w => w.UserId == userId).ToArray();
        }

        public IEnumerable<Order> GetOrdersByDateRange(DateTime? orderedFrom, DateTime? orderedTo)
        {
            if (!orderedFrom.HasValue && !orderedTo.HasValue)
                throw (new Exception("At least one parameter must be specified"));

            IEnumerable<Order> orders = _dataStore.Values;

            if (orderedFrom.HasValue)
                orders = orders.Where(w => w.OrderDate >= orderedFrom.Value);

            if (orderedTo.HasValue)
                orders = orders.Where(w => w.OrderDate <= orderedTo.Value);

            return orders;
        }
    }
}

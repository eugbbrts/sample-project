using BusinessEntities;
using Common;
using Data.Repositories;
using System;
using System.Collections.Generic;

namespace Core.Services.Orders
{
    [AutoRegister]
    public class GetOrderService : IGetOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Order GetOrder(Guid id)
        {
            return _orderRepository.Get(id);
        }

        public IEnumerable<Order> GetUserOrders(Guid userId)
        {
            return _orderRepository.GetUserOrders(userId);
        }

        public IEnumerable<Order> GetOrdersByDateRange(DateTime? createdFrom, DateTime? crearedTo)
        {
            return _orderRepository.GetOrdersByDateRange(createdFrom, crearedTo);
        }
    }
}

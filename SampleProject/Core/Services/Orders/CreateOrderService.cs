using BusinessEntities;
using Common;
using Core.Factories;
using Core.Services.Products;
using Core.Services.Users;
using Data.Repositories;
using System;
using System.Collections.Generic;

namespace Core.Services.Orders
{
    [AutoRegister]
    public class CreateOrderService : ICreateOrderService
    {
        private readonly IGetProductService _getProductService;
        private readonly IIdObjectFactory<Order> _orderFactory;
        private readonly IOrderRepository _orderRepository;
        private readonly IOrderNameService _orderNameService;
        private readonly IGetUserService _getUserService;

        public CreateOrderService(IIdObjectFactory<Order> orderFactory, IOrderRepository orderRepository, IOrderNameService orderNameService, IGetUserService getUserService, IGetProductService getProductService)
        {
            _orderFactory = orderFactory;
            _orderRepository = orderRepository;
            _orderNameService = orderNameService;
            _getProductService = getProductService;
            _getUserService = getUserService;
        }

        public Order Create(Guid id, Guid userId, IEnumerable<NewOrderItem> orderItems)
        {
            if (_orderRepository.Get(id) != null)
            {
                throw (new Exception($"Order with id {id} already exist"));
            }

            var user = _getUserService.GetUser(userId);
            if (user == null)
            {
                throw (new Exception($"User with id {userId} doesn't exist"));
            }

            var order = _orderFactory.Create(id);
            order.SetOrderNumber(_orderNameService.CreateOrderNumber());
            order.SetUserId(userId);

            foreach (var item in orderItems)
            {
                var product = _getProductService.GetProduct(item.ProductId);
                if (product == null)
                {
                    throw (new Exception($"Product with id {item.ProductId} doesn't exist"));
                }

                var orderItem = new OrderItem();
                orderItem.SetProductAndQuantity(product, item.Quantity);               

                order.AddItem(orderItem);
            }

            _orderRepository.Save(order);

            return order;
        }
    }
}

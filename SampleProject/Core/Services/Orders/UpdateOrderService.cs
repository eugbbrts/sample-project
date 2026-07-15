using BusinessEntities;
using Common;
using Common.Exceptions;
using Core.Services.Products;
using Data.Repositories;
using System;
using System.Linq;

namespace Core.Services.Orders
{
    [AutoRegister]
    public class UpdateOrderService : IUpdateOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IGetProductService _getProductService;

        public UpdateOrderService(IOrderRepository orderRepository, IGetProductService getProductService)
        {
            _orderRepository = orderRepository;
            _getProductService = getProductService;
        }

        public void RemoveProduct(Guid id, Guid productId)
        {
            var order = _orderRepository.Get(id);
            if (order == null)
                throw (new Exception($"Order with id: {id} doesn't exist"));

            order.RemoveProduct(productId);

            if (order.OrderItems.Length == 0) // delete an ordr if no product remained in it
                _orderRepository.Delete(order);

            _orderRepository.Save(order);
        }

        public void AddProduct(Guid id, Guid productId, int quantity)
        {
            var order = _orderRepository.Get(id);
            if (order == null)
                throw (new Exception($"Order with id: {id} doesn't exist"));

            if(order.Status != OrderStatus.Pending)
            {
                throw (new ValidationException("Cannot update an order in the current status."));
            }

            if (quantity < 0)
                throw (new ValidationException($"Quantity cannot be a negative number"));

            if (quantity == 0)
            {
                RemoveProduct(id, productId);
                return;
            }

            var product = _getProductService.GetProduct(productId);
            if (product == null)
            {
                throw (new Exception($"Product with id: {productId} doesn't exist."));
            }

            var orderItem = order.OrderItems.SingleOrDefault(w => w.Product.Id == productId);
            if (orderItem == null)
            {

                var newOrderItem = new OrderItem();
                newOrderItem.SetProductAndQuantity(product, quantity);

                order.AddItem(newOrderItem);
            }
            else
            {
                if (product.Price != orderItem.UnitPrice)
                {
                    throw (new ValidationException($"Cannot update product quantity, price for the product with id: {productId} has been changed."));
                }

                orderItem.SetQuantity(orderItem.Quantity + quantity);
            }

            _orderRepository.Save(order);
        }

        public void SetOrderStatus(Guid id, OrderStatus status)
        {
            var order = _orderRepository.Get(id);
            if (order == null)
                throw (new Exception($"Order with id: {id} doesn't exist"));

            order.SetOrderStatus(status);

            _orderRepository.Save(order);
        }
    }
}

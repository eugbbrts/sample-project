using BusinessEntities;
using System;

namespace Core.Services.Orders
{
    public interface IUpdateOrderService
    {
        void SetOrderStatus(Guid id, OrderStatus status);
        void RemoveProduct(Guid id, Guid productId);
        void AddProduct(Guid id, Guid productId, int quantity);
    }
}

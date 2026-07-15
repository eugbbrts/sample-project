using Common.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BusinessEntities
{
    public class Order : IdObject
    {
        private Guid _userId;
        private string _orderNumber;
        private OrderStatus _orderStatus = OrderStatus.Pending;
        private List<OrderItem> _orderItems = new List<OrderItem>();

        public string OrderNumber
        {
            get => _orderNumber;
            private set => _orderNumber = value;
        }

        public Guid UserId
        {
            get => _userId;
            private set => _userId = value;
        }

        public OrderItem[] OrderItems => _orderItems.ToArray();

        public OrderStatus Status
        {
            get => _orderStatus;
            private set => _orderStatus = value;
        }

        public DateTime OrderDate { get; private set; } = DateTime.UtcNow;       

        public decimal TotalPrice => _orderItems.Sum(s => s.TotalPrice);

        // test comments: in the real life scenarios, an order need to have some extra properties like billing/shiiping addresses, tax info and etc.

        public void AddItem(OrderItem item)
        {
            if (item.Quantity <= 0)
            {
                throw new ValidationException("Quantity must be greater than zero.");
            }

            var existingItem = _orderItems.FirstOrDefault(i => i.Product.Id == item.Product.Id);
            if (existingItem != null)
            {
                existingItem.SetQuantity(existingItem.Quantity + item.Quantity);
            }
            else
            {
                _orderItems.Add(item);
            }
        }

        public void RemoveProduct(Guid productId)
        {
            var orderItem = _orderItems.FirstOrDefault(i => i.Product.Id == productId);

            _orderItems.Remove(orderItem);
        }

        public void SetOrderNumber(string orderNumber)
        {
            if (String.IsNullOrWhiteSpace(orderNumber))
            {
                throw new ValidationException("Order number cannot be empty");
            }

            _orderNumber = orderNumber;
        }

        public void SetOrderStatus(OrderStatus orderStatus)
        {
            if (_orderStatus == OrderStatus.Cancelled)
            {
                throw new ValidationException("Cannot update cancelled order.");
            }

            _orderStatus = orderStatus;
        }

        public void SetUserId(Guid userId)
        {
            _userId = userId;
        }
    }
}

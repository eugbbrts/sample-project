using BusinessEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebApi.Models.Orders
{
    public class OrderData : IdObjectData
    {
        public OrderData(Order order) : base(order)
        {
            Status = order.Status;
            OrderNumber = order.OrderNumber;
            UserId = order.UserId;
            OrderDate = order.OrderDate;

            OrderItems = order.OrderItems?.Select(s => new OrderItemData(s)).ToArray() ?? new OrderItemData[0];
        }

        public OrderStatus Status { get; set; }
        public string OrderNumber { get; set; }
        public Guid UserId { get; set; }
        public DateTime OrderDate { get; set; }

        public IEnumerable<OrderItemData> OrderItems { get; set; }
    }
}
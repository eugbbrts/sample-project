using System;

namespace WebApi.Models.Orders
{
    public class AddOrderItemModel
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
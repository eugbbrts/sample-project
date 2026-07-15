using System;
using System.Collections.Generic;

namespace WebApi.Models.Orders
{
    public class CreateOrderModel
    {
        public IEnumerable<AddOrderItemModel> OrderItems { get; set; }
    }
}
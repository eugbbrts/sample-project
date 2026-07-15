using System;

namespace BusinessEntities
{
    public class NewOrderItem
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
using BusinessEntities;
using WebApi.Models.Products;

namespace WebApi.Models.Orders
{
    public class OrderItemData : IdObjectData
    {
        public OrderItemData(OrderItem orderItem) : base(orderItem)
        {
            Product = new ProductData(orderItem.Product);
            Quantity = orderItem.Quantity;
            UnitPrice = orderItem.UnitPrice;
        }

        public ProductData Product { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
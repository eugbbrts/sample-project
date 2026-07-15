using BusinessEntities;

namespace WebApi.Models.Products
{
    public class ProductData : IdObjectData
    {
        public ProductData(Product product) : base(product)
        {
            Category = product.Category; 
            Name = product.Name;
            Sku = product.Sku;
            Price = product.Price;
        }

        public string Category { get; set; }
        public string Name { get; set; }
        public string Sku { get; set; }
        public decimal Price { get; set; }
    }
}
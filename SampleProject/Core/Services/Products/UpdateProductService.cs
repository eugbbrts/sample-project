using BusinessEntities;
using Common;

namespace Core.Services.Products
{
    [AutoRegister]
    public class UpdateProductService : IUpdateProductService
    {
        public void Update(Product product, string category, string name, string sku, decimal price)
        {
            product.SetCategory(category);
            product.SetName(name);
            product.SetSku(sku);
            product.SetPrice(price);
        }
    }
}

using BusinessEntities;
using System;
using System.Collections.Generic;

namespace Core.Services.Products
{
    public interface IGetProductService
    {
        Product GetProduct(Guid id);

        IEnumerable<Product> GetProducts(string category = null, string name = null, string sku = null, decimal? priceFrom = null, decimal? priceTo = null);
    }
}

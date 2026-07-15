using BusinessEntities;
using System;
using System.Collections.Generic;

namespace Data.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        bool Exists(Guid id);

        IEnumerable<Product> GetProducts(string category = null, string name = null, string sku = null, decimal? priceFrom = null, decimal? priceTo = null);
    }
}

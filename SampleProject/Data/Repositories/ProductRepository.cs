using BusinessEntities;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Data.Repositories
{
    [AutoRegister]
    public class ProductRepository : InMemoryRepository<Product>, IProductRepository
    {
        public bool Exists(Guid id)
        {
            return _dataStore.ContainsKey(id);
        }

        public IEnumerable<Product> GetProducts(string category = null, string name = null, string sku = null, decimal? priceFrom = null, decimal? priceTo = null)
        {
            var query = _dataStore.Values.AsEnumerable<Product>();

            if (!String.IsNullOrWhiteSpace(category))
            {
                var lowerCategory = category.ToLower();
                query = query.Where(w => w.Category.ToLower() == lowerCategory);
            }

            if (!String.IsNullOrWhiteSpace(sku))
            {
                var lowerSku = sku.ToLower().Trim();
                query = query.Where(w => w.Sku.ToLower() == lowerSku);
            }

            if (!String.IsNullOrWhiteSpace(name))
            {
                var lowerName = name.ToLower();
                query = query.Where(w => w.Name.ToLower().Contains(lowerName));
            }

            if (priceFrom.HasValue)
            {
                query = query.Where(w => w.Price >= priceFrom.Value);
            }

            if (priceTo.HasValue)
            {
                query = query.Where(w => w.Price <= priceTo.Value);
            }

            return query;
        }
    }
}

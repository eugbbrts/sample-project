using BusinessEntities;
using System;

namespace Core.Services.Products
{
    public interface ICreateProductService
    {
        Product Create(Guid id, string category, string name, string sku, decimal price);
    }
}

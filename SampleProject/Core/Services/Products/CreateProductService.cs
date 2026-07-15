using BusinessEntities;
using Common;
using Core.Factories;
using Data.Repositories;
using System;
using System.Linq;

namespace Core.Services.Products
{
    [AutoRegister]
    public class CreateProductService : ICreateProductService
    {
        private readonly IUpdateProductService _updateProductService;
        private readonly IIdObjectFactory<Product> _productFactory;
        private readonly IProductRepository _productRepository;

        public CreateProductService(IIdObjectFactory<Product> productFactory, IProductRepository productRepository, IUpdateProductService updateProductService)
        {
            _productFactory = productFactory;
            _productRepository = productRepository;
            _updateProductService = updateProductService;
        }

        public Product Create(Guid id, string category, string name, string sku, decimal price)
        {
            if (_productRepository.Get(id) != null)
            {
                throw (new Exception($"Product with id: {id} already exists"));
            }

            if (_productRepository.GetProducts(sku: sku).Any())
            {
                throw (new Exception($"Product with sku: {sku} already exists"));
            }

            var product = _productFactory.Create(id);
            _updateProductService.Update(product, category, name, sku, price);
            _productRepository.Save(product);

            return product;
        }
    }
}

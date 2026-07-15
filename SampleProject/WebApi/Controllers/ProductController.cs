using Common.Exceptions;
using Core.Services.Products;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models.Products;

namespace WebApi.Controllers
{
    // Test comments: we assume the controller allows anonymous access only because of it is a part of a test app and we ignore the proper authentication and authorization
    [RoutePrefix("products")]
    public class ProductController : BaseApiController
    {
        private const int PRODUCTS_DEFAULT_TAKE = 20;
        private const int PRODUCTS_MAX_TAKE = 100;

        private readonly ICreateProductService _createProductService;
        private readonly IDeleteProductService _deleteProductService;
        private readonly IGetProductService _getProductService;
        private readonly IUpdateProductService _updateProductService;

        public ProductController(ICreateProductService createProductService, IDeleteProductService deleteProductService, IGetProductService getProductService, IUpdateProductService updateProductService)
        {
            _createProductService = createProductService;
            _deleteProductService = deleteProductService;
            _getProductService = getProductService;
            _updateProductService = updateProductService;
        }

        // test comments: we allow to create a product with an ID from the request intput, only for the test purposes
        [Route("{productId:guid}/create")]
        [HttpPost]
        public HttpResponseMessage CreateProduct(Guid productId, [FromBody] ProductModel model)
        {
            // Test comments: it is good practice to use declarative validation rules for models (see the changes in the ProductModel class)
            (var validationResult, var errorMessage) = ValidateModel(model);
            if (!validationResult)
                return ValidationFails(errorMessage);

            var product = _createProductService.Create(productId, model.Category, model.Name, model.Sku, model.Price);
            return Found(new ProductData(product));
        }

        [Route("{productId:guid}/update")]
        [HttpPost]
        public HttpResponseMessage UpdateProduct(Guid productId, [FromBody] ProductModel model)
        {
            var product = _getProductService.GetProduct(productId);
            if (product == null)
            {
                return DoesNotExist();
            }

            (var validationResult, var errorMessage) = ValidateModel(model);
            if (!validationResult)
                return ValidationFails(errorMessage);

            try
            {
                _updateProductService.Update(product, model.Category, model.Name, model.Sku, model.Price);
                return Found(new ProductData(product));
            }
            catch (ValidationException ex)
            {
                return ValidationFails(ex.Message);
            }
        }

        [Route("{productId:guid}/delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteProduct(Guid productId)
        {
            var product = _getProductService.GetProduct(productId);
            if (product == null)
            {
                return DoesNotExist();
            }
            _deleteProductService.Delete(product);
            return Found();
        }

        [Route("{productId:guid}")]
        [HttpGet]
        public HttpResponseMessage GetProduct(Guid productId)
        {
            var product = _getProductService.GetProduct(productId);
            if (product == null)
            {
                return DoesNotExist();
            }

            return Found(new ProductData(product));
        }

        [Route("list")]
        [HttpGet]
        public HttpResponseMessage GetProducts(int skip = 0, int take = PRODUCTS_DEFAULT_TAKE, string category = null, string name = null, string sku = null, decimal? priceFrom = null, decimal? priceTo = null)
        {
            AssertParams(take, PRODUCTS_MAX_TAKE);

            var products = _getProductService.GetProducts(category, name, sku, priceFrom, priceTo)
                                       .Skip(skip).Take(take)
                                       .Select(q => new ProductData(q))
                                       .ToList();
            return Found(products);
        }
    }
}
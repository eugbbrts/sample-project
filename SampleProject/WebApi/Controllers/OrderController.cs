using BusinessEntities;
using Common.Exceptions;
using Core.Services.Orders;
using System;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models.Orders;

namespace WebApi.Controllers
{
    // Test comments: we assume the controller allows anonymous access only because of it is a part of a test app and we ignore the proper authentication and authorization
    [RoutePrefix("orders")]
    public class OrderController : BaseApiController
    {
        private const int ORDERS_DEFAULT_TAKE = 20;
        private const int ORDERS_MAX_TAKE = 100;

        private readonly ICreateOrderService _createOrderService;
        private readonly IDeleteOrderService _deleteOrderService;
        private readonly IGetOrderService _getOrderService;
        private readonly IUpdateOrderService _updateOrderService;

        public OrderController(ICreateOrderService createOrderService, IDeleteOrderService deleteOrderService, IGetOrderService getOrderService, IUpdateOrderService updateOrderService)
        {
            _createOrderService = createOrderService;
            _deleteOrderService = deleteOrderService;
            _getOrderService = getOrderService;
            _updateOrderService = updateOrderService;
        }

        // test comments: we allow to create an order with an ID from the request intput, only for the test purposes
        [Route("{orderId:guid}/create")]
        [HttpPost]
        public HttpResponseMessage CreateOrder(Guid orderId, [FromBody] CreateOrderModel model)
        {
            // Test comments: it is good practice to use declarative validation rules for models (see the changes in the OrderModel class)
            (var validationResult, var errorMessage) = ValidateModel(model);
            if (!validationResult)
            {
                return ValidationFails(errorMessage);
            }

            if (!model.OrderItems.Any())
            {
                return ValidationFails("At least one product must be included into a new order.");
            }

            var orderItems = model.OrderItems.Select(s => new NewOrderItem()
            {
                ProductId = s.ProductId,
                Quantity = s.Quantity,
            });

            var order = _createOrderService.Create(orderId, GetCurrentUserId(), orderItems);
            return Found(new OrderData(order));
        }

        [Route("{orderId:guid}/cancelorder")]
        [HttpPost]
        public HttpResponseMessage CancelOrder(Guid orderId)
        {
            var order = _getOrderService.GetOrder(orderId);
            if (order == null)
            {
                return DoesNotExist();
            }

            if (!AllowedToUpdateTheOrder(order))
                return NotAuthorized();

            switch (order.Status)
            {
                case OrderStatus.Cancelled:
                    return ValidationFails("Already canceled");
                case OrderStatus.Delivered:
                    return ValidationFails("Cannot cancel delivered order");
                case OrderStatus.Shipped:
                    return ValidationFails("Cannot cancel shipped order");
            }

            _updateOrderService.SetOrderStatus(order.Id, OrderStatus.Cancelled);

            return GetOrder(order.Id);
        }

        [Route("{orderId:guid}/addproduct")]
        [HttpPost]
        public HttpResponseMessage AddProduct(Guid orderId, [FromBody] AddOrderItemModel model)
        {
            var order = _getOrderService.GetOrder(orderId);
            if (order == null)
            {
                return DoesNotExist();
            }

            if (!AllowedToUpdateTheOrder(order))
                return NotAuthorized();

            (var validationResult, var errorMessage) = ValidateModel(model);
            if (!validationResult)
                return ValidationFails(errorMessage);

            try
            {
                _updateOrderService.AddProduct(order.Id, model.ProductId, model.Quantity);

                return GetOrder(order.Id);
            }
            catch (ValidationException ex)
            {
                return ValidationFails(ex.Message);
            }
        }

        [Route("{orderId:guid}/delete")]
        [HttpDelete]
        public HttpResponseMessage DeleteOrder(Guid orderId)
        {
            var order = _getOrderService.GetOrder(orderId);
            if (order == null)
            {
                return DoesNotExist();
            }
            _deleteOrderService.Delete(order);
            return Found();
        }

        [Route("{orderId:guid}")]
        [HttpGet]
        public HttpResponseMessage GetOrder(Guid orderId)
        {
            var order = _getOrderService.GetOrder(orderId);
            return Found(new OrderData(order));
        }

        // Test comments: return list of orders for a correct user
        [Route("list")]
        [HttpGet]
        public HttpResponseMessage GetOrders(int skip = 0, int take = ORDERS_DEFAULT_TAKE, string name = null, bool ascending = false)
        {
            AssertParams(take, ORDERS_MAX_TAKE);

            var orders = _getOrderService.GetUserOrders(GetCurrentUserId())
                                       .Skip(skip).Take(take)
                                       .Select(q => new OrderData(q));

            orders = ascending ? orders.OrderBy(o => o.OrderDate) : orders.OrderByDescending(o => o.OrderDate);

            return Found(orders.ToList());
        }

        private bool AllowedToUpdateTheOrder(Order order)
        {
            return order.UserId == GetCurrentUserId();
        }
    }
}
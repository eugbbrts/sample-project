using Common;

namespace Core.Services.Orders
{
    // test comments: user friendly order number service, for test purposes just an autoincrement becauseof in-memory storage
    [AutoRegister]
    public class OrderNameService : IOrderNameService
    {
        private static object _orderNumberLock = new object();
        private static int _orderNumber = 1;
        public string CreateOrderNumber()
        {
            lock (_orderNumberLock)
            {
                return $"ORDER-{_orderNumber++}";
            }
        }
    }
}

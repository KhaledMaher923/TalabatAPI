using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core.Entities.Order_Aggregate;

namespace Talabat.Core.Services
{
    public interface IOrderService
    {
        public Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShppingAddress);
        public Task<IReadOnlyCollection<Order>> GetOrdersForSpecificuserAsync(string BuyerEmail);
        public Task<Order> GetOrderByIdForSpecificUserAsync(string BuyerEmail, int OrderId);
    }
}

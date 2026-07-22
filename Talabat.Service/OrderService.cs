using System;
using System.Collections.Generic;
using System.Text;
using Talabat.Core;
using Talabat.Core.Entities;
using Talabat.Core.Entities.Order_Aggregate;
using Talabat.Core.Repositories;
using Talabat.Core.Services;

namespace Talabat.Service
{
    public class OrderService : IOrderService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;

        public OrderService(IBasketRepository basketRepository,
            IUnitOfWork unitOfWork)
        {
            _basketRepository = basketRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Order?> CreateOrderAsync(string BuyerEmail, string BasketId, int DeliveryMethodId, Address ShppingAddress)
        {
            var Basket = await _basketRepository.GetBasketAsync(BasketId);

            var OrderItems = new List<OrderItem>();
            if(Basket?.Items.Count > 0)
            {
                foreach(var item in Basket.Items)
                {
                    var Product = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);
                    var ProductItemOrdered = new ProductItemOrdered(Product.Id, Product.Name, Product.PictureUrl);
                    var OrderItem = new OrderItem(ProductItemOrdered, item.Quantity, Product.Price);
                    OrderItems.Add(OrderItem);

                }
            }
        
            var SubTotal = OrderItems.Sum(item => item.Price * item.Quantity);

            var DeliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByIdAsync(DeliveryMethodId);
        
            var Order = new Order(BuyerEmail, ShppingAddress, DeliveryMethod, OrderItems, SubTotal);

            await _unitOfWork.Repository<Order>().AddAsync(Order);

            var Result = await _unitOfWork.CompleteAsync();
            if (Result <= 0) return null;
            return Order;

        }

        public Task<Order> GetOrderByIdForSpecificUserAsync(string BuyerEmail, int OrderId)
        {
            throw new NotImplementedException();
        }

        public Task<IReadOnlyCollection<Order>> GetOrdersForSpecificuserAsync(string BuyerEmail)
        {
            throw new NotImplementedException();
        }
    }
}

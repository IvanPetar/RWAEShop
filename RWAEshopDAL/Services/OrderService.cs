using RWAEshopDAL.Models;
using RWAEshopDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWAEshopDAL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUserRepository _userRepository;

        public OrderService(IOrderRepository orderRepository, IProductRepository productRepository, IUserRepository userRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public void CreateOrder(Order order)=> _orderRepository.Add(order);

        public void DeleteOrder(int id)=> _orderRepository.Delete(id);

        public IEnumerable<Order> GetAllOrders()=> _orderRepository.GetAll();

        public Order? GetOrder(int id)=> _orderRepository.GetById(id);

        public void UpdateOrder(Order order) 
        {
            _orderRepository.Update(order);
        }

        public void UpdateOrderItem(int id, int productId, int quantity)
        {
            var order = _orderRepository.GetById(id);
            if (order == null) {
                throw new ArgumentException("Order not found for specific user");
            }
            var itemToUpdate = order.OrderItems.FirstOrDefault(i =>
                i.ProductId == productId);
            if (itemToUpdate != null)
            {
                itemToUpdate.Quantity = quantity;
                _orderRepository.Update(order);
            }
            else
            {
                throw new ArgumentException("Item bot found in cart/order");
            }
        }
    }
}

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

        public void UpdateOrder(int userId, int productId, int quantity) 
        {
            var order = _orderRepository.GetById(userId);
            if (order == null)
                throw new ArgumentException("Order not found for that specific user");

            var itemToUpdate = order.OrderItems.FirstOrDefault(i => i.ProductId == productId);
            if (itemToUpdate == null)
                throw new ArgumentException("item not found in that order");
            itemToUpdate.Quantity = quantity;

            decimal newTotal = 0;

            foreach (var item in order.OrderItems)
            {
                var product = _productRepository.GetById(item.ProductId);
                if (product == null)
                    throw new ArgumentException($"Product with that {item.ProductId} doesnt exist");

                newTotal = product.Price * item.Quantity;
            }

            order.TotalAmount = newTotal;

           _orderRepository.Update(order);
           _orderRepository.SaveChanges();
        }
    }
}

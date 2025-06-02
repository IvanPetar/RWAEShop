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

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void CreateOrder(Order order)=> _orderRepository.Add(order);

        public void DeleteOrder(int id)=> _orderRepository.Delete(id);

        public IEnumerable<Order> GetAllOrders()=> _orderRepository.GetAll();

        public Order? GetOrder(int id)=> _orderRepository.GetById(id);

        public void UpdateOrder(Order order)=> _orderRepository.Update(order);
    }
}

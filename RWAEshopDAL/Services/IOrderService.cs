using RWAEshopDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWAEshopDAL.Services
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAllOrders();
        Order? GetOrder(int id);
        void CreateOrder(Order order);
        void UpdateOrder(int userId, int productId, int quantity);
        void DeleteOrder(int id);
    }
}

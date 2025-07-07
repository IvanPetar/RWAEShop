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
        void UpdateOrder(Order order);
        void UpdateOrderItem(int id, int productId, int quantity);
        void RemoveOrderItem(int id, int productId);
        void DeleteOrder(int id);

        bool IsProductInAnyOrder(int productId);
    }
}

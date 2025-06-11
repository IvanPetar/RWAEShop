using Microsoft.EntityFrameworkCore;
using RWAEshopDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWAEshopDAL.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly EshopContext _context;

        public OrderRepository(EshopContext context)
        {
            _context = context;
        }

        public void Add(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _context.Orders
                .Include(o=> o.OrderItems)
                .FirstOrDefault(o => o.IdOrder == id);

            if (item != null) 
            {
                _context.OrderItems.RemoveRange(item.OrderItems);
                _context.Orders.Remove(item);

                _context.SaveChanges();
            }
        }

        public IEnumerable<Order> GetAll()
        {
            return _context.Orders.Include(oi => oi.OrderItems).ToList();
        }

        public Order? GetById(int id)
        {
            return _context.Orders.Include(oi => oi.OrderItems).FirstOrDefault(oi => oi.IdOrder == id);
        }

        public void Update(Order order)=>_context.Orders.Update(order);
        public void SaveChanges()=> _context.SaveChanges();
      
    }
}

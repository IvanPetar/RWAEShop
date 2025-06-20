using Microsoft.EntityFrameworkCore;
using RWAEshopDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWAEshopDAL.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EshopContext _context;

        public UserRepository(EshopContext context)
        {
            _context = context;
        }

        public void Add(User user)
        {
            user.RoleId = 2;
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        public void Delete(int id)
        {
            var item = _context.Users
                .Include(i => i.Orders)
                .FirstOrDefault(i=> i.IdUser == id);

            if (item != null) 
            {
                _context.Orders.RemoveRange(item.Orders);
                _context.Users.Remove(item);

                _context.SaveChanges();
            }
        }

        public IEnumerable<User> GetAll()
        {
            return _context.Users
                .Include(u => u.Role)
                .ToList();
        }

        public User? GetById(int id)
        {
            return _context.Users.Find(id);
        }

        public void Update(User user)
        {
            _context.Users.Update(user);
            _context.SaveChanges();
        }
    }
}

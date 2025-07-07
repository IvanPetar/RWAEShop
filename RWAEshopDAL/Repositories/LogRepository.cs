using RWAEshopDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWAEshopDAL.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly EshopContext _context;

        public LogRepository(EshopContext context)
        {
            _context = context;
        }


        public void Add(Log entry) => _context.Logs.Add(entry);
        public void Save() => _context.SaveChanges();

        public IEnumerable<Log> GetLastN(int count) =>
            _context.Logs.OrderByDescending(l => l.Timestamp).Take(count).ToList();

        public int Count() => _context.Logs.Count();

        public IEnumerable<Log> GetAll()
        => _context.Logs.OrderByDescending(l => l.Timestamp).ToList();

        public Log? GetById(int id)
        => _context.Logs.FirstOrDefault(l => l.Id == id);

        public IQueryable<Log> GetQueryable()
        {
            return _context.Logs.OrderByDescending(l => l.Timestamp).AsQueryable();
        }
    }
}

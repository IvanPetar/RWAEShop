using RWAEshopDAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWAEshopDAL.Services
{
    public interface ILogService
    {
        Log Log(string message, int level = 1);
        void Add(Log entry);
        void Save();
        IEnumerable<Log> GetLastN(int count);
        int Count();
        IEnumerable<Log> GetAll();
        Log? GetById(int id);
        IQueryable<Log> GetAllQueryable();
    }
}

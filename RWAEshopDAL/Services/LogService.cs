using RWAEshopDAL.Models;
using RWAEshopDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWAEshopDAL.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepo;

        public LogService(ILogRepository logRepo)
        {
            _logRepo = logRepo;
        }

        public Log Log(string message, int level = 1)
        {
            var entry = new Log
            {
                Message = message,
                Level = level,
                Timestamp = DateTime.UtcNow
            };

            _logRepo.Add(entry);
            _logRepo.Save();
            return entry;
        }

        public IEnumerable<Log> GetLastN(int count)
        {
            return _logRepo.GetLastN(count);
        }

        public int Count()
        {
            return _logRepo.Count();
        }

        public IEnumerable<Log> GetAll()
        {
            return _logRepo.GetAll();
        }

        public Log? GetById(int id)
        {
            return _logRepo.GetById(id);
        }
        public IQueryable<Log> GetAllQueryable()
        {
            return _logRepo.GetQueryable();
        }

        public void Add(Log entry)
        {
            _logRepo.Add(entry);
        }

        public void Save()
        {
            _logRepo.Save();
        }
    }
}

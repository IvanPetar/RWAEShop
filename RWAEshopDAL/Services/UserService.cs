using RWAEshopDAL.Models;
using RWAEshopDAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RWAEshopDAL.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository)
        {
            _repository = repository;
        }

        public void CreateUser(User user)=> _repository.Add(user);

        public void DeleteUser(int id)=> _repository.Delete(id);
        public IEnumerable<User> GetAllUsers()=> _repository.GetAll();
        public User? GetUser(int id)=> _repository.GetById(id);
        public void UpdateUser(User user)=> _repository.Update(user);
    }
}

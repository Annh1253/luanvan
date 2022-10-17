using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Contracts.RepositoryContracts;
using UserService.Data;
using UserService.Models;


namespace UserService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContext _context;

        public UserRepository(DBContext context)
        {
            this._context = context;
        }
        public bool AddUser(User user)
        {
            _context.Users.Add(user);
            return SaveChanges();
        }

        public bool Exist(int id)
        {
            return _context.Users.Any(user => user.Id == id);
        }

        public bool Exist(string email)
        {
            return _context.Users.Any(user => user.Email == email);
        }

        public ICollection<User> GetAllUsers()
        {
           
            return _context.Users.ToList();
        }

        public User GetByEmail(string email)
        {
            return _context.Users.FirstOrDefault(user => user.Email == email);
        }

        public User GetById(int id)
        {
            return _context.Users.FirstOrDefault(user => user.Id == id);
        }

        public bool UpdateUser(User user)
        {
            _context.Users.Update(user);
            return SaveChanges();
        }

        public bool DeleteUser(User user)
        {
            _context.Users.Remove(user);
            return SaveChanges();
        }
        
        public User GetByName(string name)
        {
            throw new NotImplementedException();
        }

        private bool SaveChanges()
        {
            return _context.SaveChanges() >=0;
        }
    }
}
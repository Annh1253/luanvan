using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Contracts.RepositoryContracts
{
    public interface IUserRepository
    {
        bool UpdateUser(User user);
        bool DeleteUser(User user);
        ICollection<User> GetAllUsers();

        User GetById(int id);

        bool AddUser(User user);

        User GetByName(string name);

        User GetByEmail(string email); 

        bool Exist(int id);

        bool Exist(string email);
    }
}
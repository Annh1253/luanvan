using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Models;

namespace UserService.Contracts.RepositoryContracts
{
    public interface IRoleRepository
    {
        ICollection<Role> GetAllRoles();
        Role GetById(int id);

        bool AddRole(Role role);

        Role GetByName(string name);
      
        bool Exist(int id);

    }
}

using UserService.Contracts.RepositoryContracts;
using UserService.Data;
using UserService.Models;

namespace UserService.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly DBContext _context;

        public RoleRepository(DBContext context)
        {
            this._context = context;
        }
        public bool AddRole(Role role)
        {
            _context.Roles.Add(role);
            return SaveChanges();
        }

        public bool Exist(int id)
        {
            return _context.Roles.Any(r => r.Id == id);
        }

        public ICollection<Role> GetAllRoles()
        {
            return _context.Roles.ToList();
        }

        public Role GetById(int id)
        {
            return _context.Roles.FirstOrDefault(r => r.Id ==id);
        }

        public Role GetByName(string name)
        {
            return _context.Roles.FirstOrDefault(r => r.Name == name);
        }
        
        private bool SaveChanges()
        {
            return _context.SaveChanges() >=0;
        }
    }
}
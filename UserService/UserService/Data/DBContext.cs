using UserService.Models;
using Microsoft.EntityFrameworkCore;

namespace UserService.Data
{
    public class DBContext
    {
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
            
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
    }
}
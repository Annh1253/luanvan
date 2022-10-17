
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using UserService.Models;
namespace UserService.Models
{
    public class User : BaseEntity
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int RoleId { get; set; }

        public List<Role> Roles = new List<Role>();
     
    }
}
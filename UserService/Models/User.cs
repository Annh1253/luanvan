using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UserService.Models
{
    public class User
    {
        public int Id { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        private List<Role> _roles = new List<Role>();
        protected ILazyLoader LazyLoader { get; set; }
        public List<Role> Roles
        {
            get => LazyLoader.Load(this, ref _roles);
            set => _roles = value;
        }

    }
}
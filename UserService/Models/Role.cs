using Microsoft.EntityFrameworkCore.Infrastructure;

namespace UserService.Models
{
    public class Role
    {
        public static readonly string DEFAULT_ROLE_NAME = "Contestant";   
        public int Id { get; set; }
        public string Name { get; set; } = DEFAULT_ROLE_NAME;

        

        public List<User> _users = new List<User>();
        
        protected ILazyLoader LazyLoader { get; set; }
        public List<User> Users
        {
            get => LazyLoader.Load(this, ref _users);
            set => _users = value;
        }
    }
}
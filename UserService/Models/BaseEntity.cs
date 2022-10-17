using Microsoft.EntityFrameworkCore.Infrastructure;

namespace UserService.Models
{
     public abstract class BaseEntity
    {
        protected ILazyLoader LazyLoader { get; set; }
    }
}
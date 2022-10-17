
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace UserService.Models
{
    public class BaseEntity
    {
         protected ILazyLoader LazyLoader { get; set; }
    }
}
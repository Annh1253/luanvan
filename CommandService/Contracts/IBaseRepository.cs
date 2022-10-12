using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CommandService.Contracts
{
    public interface IBaseRepository<T>
    {
        void Create(T item);
        IEnumerable<T> GetAll();
        
    }
}
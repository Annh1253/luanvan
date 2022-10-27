using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Contracts.RepositoryContracts;
using ExamService.Models;
using UserService.Data;

namespace ExamService.Repositories
{
    public class OptionRepository : IOptionRepository
    {

       private readonly DBContext _dbContext;

        public OptionRepository(DBContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public bool AddOption(Option option)
        {
            _dbContext.Options.Add(option);
            return SaveChanges();
        }

        public bool Exist(int id)
        {
            return _dbContext.Options.Any(o => o.Id == id);
        }

        public List<Option> GetAllOptions()
        {
           return _dbContext.Options.ToList();
        }

        public Option GetById(int id)
        {
           return _dbContext.Options.FirstOrDefault(o => o.Id == id);
        }

       

        private Boolean SaveChanges()
        {
            return this._dbContext.SaveChanges()>0;
        }
    }
}
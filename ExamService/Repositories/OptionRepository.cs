using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Contracts.RepositoryContracts;
using ExamService.Dtos;
using ExamService.Helpers;
using ExamService.Models;
using ExamService.Data;

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
            Console.WriteLine("Saving option");
            try
            {
                _dbContext.Options.Add(option);
                return SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
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

        public bool RemoveOption(Option question)
        {
            try
            {
                _dbContext.Options.Remove(question);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return SaveChanges();
        }

        public bool UpdateOption(int OldOptionId, OptionUpdateRequestDto newOption)
        {
            try
            {
                Option oldOption = _dbContext.Options.First(o => o.Id == OldOptionId);
                CRUDHelper.CopyNoneNull(newOption, oldOption);

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return SaveChanges();
        }

        private Boolean SaveChanges()
        {
            return this._dbContext.SaveChanges() > 0;
        }
    }
}
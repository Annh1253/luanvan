using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Contracts.RepositoryContracts;
using ExamService.Data;
using ExamService.Models;

namespace ExamService.Repositories
{
    public class AnswerRepository : IAnswerRepository
    {
        private readonly DBContext _dbContext;

        public AnswerRepository(DBContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public bool AddAnswer(Answer Answer)
        {
            try
            {
                _dbContext.Answers.Add(Answer);
                return SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AddAnswers(List<Answer> Answers)
        {
            try
            {
                _dbContext.Answers.AddRange(Answers);
                return SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Exist(int id)
        {
            return _dbContext.Answers.Any(answer => answer.Id == id);
        }

        public List<Answer> GetAllAnswers()
        {
            try
            {
                return _dbContext.Answers.ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public Answer GetById(int id)
        {
            try
            {
                return _dbContext.Answers.FirstOrDefault(answer => answer.Id == id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveChanges()
        {
            return _dbContext.SaveChanges() > 0;
        }
    }
}
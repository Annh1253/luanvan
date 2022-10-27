using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Contracts.RepositoryContracts;
using ExamService.Models;
using UserService.Data;

namespace ExamService.Repositories
{
    public class QuestionRepository : IQuestionRepository
    {
        private readonly DBContext _dbContext;

        public QuestionRepository(DBContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public bool AddQuestion(Question question)
        {
            _dbContext.Questions.Add(question);
            return SaveChanges();
        }

        public bool Exist(int id)
        {
            return _dbContext.Questions.Any(q => q.Id == id);
        }

        public List<Question> GetAllQuestions()
        {
           return _dbContext.Questions.ToList();
        }

        public Question GetById(int id)
        {
           return _dbContext.Questions.FirstOrDefault(q => q.Id == id);
        }

       
         private Boolean SaveChanges()
        {
            return this._dbContext.SaveChanges()>0;
        }
    }
}
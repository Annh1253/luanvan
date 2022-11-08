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
    public class QuestionRepository : IQuestionRepository
    {
        private readonly DBContext _dbContext;

        public QuestionRepository(DBContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public bool AddQuestion(Question question)
        {
            try{
                _dbContext.Questions.Add(question);
            }catch(Exception ex)
            {
                    return false;
            }
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

         public bool UpdateQuestion(int OldQuestionId , QuestionUpdateRequestDto newQuestion){
            try{
                 Question oldQuestion = _dbContext.Questions.First(e => e.Id == OldQuestionId);
                CRUDHelper.CopyNoneNull(newQuestion, oldQuestion);
                return SaveChanges();
            }catch(Exception ex){
                throw ex;   
            }
        }

        public bool RemoveQuestion(Question question)
        {
            try{
                _dbContext.Questions.Remove(question);
            }catch(Exception ex)
            {
                return false;
            }
            return SaveChanges();
        }
    }
}
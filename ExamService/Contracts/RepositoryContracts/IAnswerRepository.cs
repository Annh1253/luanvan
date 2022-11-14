using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Models;

namespace ExamService.Contracts.RepositoryContracts
{
    public interface IAnswerRepository
    {
         List<Answer> GetAllAnswers();

        Answer GetById(int id);

        bool AddAnswer(Answer Answer);

        bool AddAnswers(List<Answer> Answers);

      
        bool Exist(int id);
    }
}
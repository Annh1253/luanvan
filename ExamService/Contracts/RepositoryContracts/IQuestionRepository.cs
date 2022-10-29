using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Dtos;
using ExamService.Models;

namespace ExamService.Contracts.RepositoryContracts
{
    public interface IQuestionRepository
    {
        List<Question> GetAllQuestions();
        Question GetById(int id);

        bool AddQuestion(Question question);
        public bool UpdateQuestion(int OldQuestionId , QuestionUpdateRequestDto newQuestion);
        public bool RemoveQuestion(Question question);
        bool Exist(int id);
    }
}
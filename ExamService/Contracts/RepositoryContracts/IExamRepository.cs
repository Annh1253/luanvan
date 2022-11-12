using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Dtos;
using ExamService.Models;

namespace ExamService.Contracts.RepositoryContracts
{
    public interface IExamRepository
    {
        List<Exam> GetAllExams();
        List<Exam> GetAllExamsWithNoQuestion();

        List<Exam> GetAllExamsByTopic(Topic topic);
        List<Exam> GetAllExamsByAuthor(String AuthorEmail);


        Exam GetById(int id);
        Exam GetByEmail(string email);
        bool AddExam(Exam exam);

        bool UpdateExam(int oldExamId, ExamUpdateRequestDto newExam);

        bool RemoveExam(Exam exam);

        Exam GetByName(string name);
      
        bool Exist(int id);
    
    }
}
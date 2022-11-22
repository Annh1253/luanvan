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
    public class ExamRepository : IExamRepository
    {
        private readonly DBContext _dbContext;

        public ExamRepository(DBContext context)
        {
            this._dbContext = context;
        }

        public bool AddExam(Exam exam)
        {
            _dbContext.Exams.Add(exam);
            return SaveChanges();
        }

        public bool Exist(int id)
        {
            return _dbContext.Exams.Any(e => e.Id == id);
        }

        public List<Exam> GetAllExams()
        {
            List<Exam> examList = _dbContext.Exams.Where(exam => exam.Questions.Count() > 0).ToList();
            return examList;
        }

        public Exam GetById(int id)
        {
            return _dbContext.Exams.FirstOrDefault(e => e.Id == id);
        }

        public Exam GetByName(string name)
        {
            return _dbContext.Exams.FirstOrDefault(e => e.Name == name);
        }

        public Exam GetByEmail(string email)
        {
            return _dbContext.Exams.FirstOrDefault(e => e.AuthorEmail == email);
        }

        public bool UpdateExam(int OldExamId, ExamUpdateRequestDto newExam)
        {
            Exam oldExam = _dbContext.Exams.First(e => e.Id == OldExamId);
            CRUDHelper.CopyNoneNull(newExam, oldExam);
            return SaveChanges();
        }

        public bool RemoveExam(Exam exam)
        {

            _dbContext.Exams.Remove(exam);
            return SaveChanges();
        }

        private Boolean SaveChanges()
        {
            return this._dbContext.SaveChanges() > 0;
        }

        public List<Exam> GetAllExamsByTopic(Topic topic)
        {
            return this._dbContext.Exams.Where(exam => exam.Topic == topic).ToList();
        }

        public List<Exam> GetAllExamsByAuthor(string AuthorEmail)
        {
            return this._dbContext.Exams.Where(exam => exam.AuthorEmail == AuthorEmail).ToList();
        }

        public List<Exam> GetAllExamsWithNoQuestion()
        {
            List<Exam> examList = _dbContext.Exams.Where(exam => exam.Questions.Count() == 0).ToList();
            return examList;
        }
    }
}
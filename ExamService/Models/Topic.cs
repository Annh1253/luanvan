using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ExamService.Models
{
    public class Topic
    {
        public int Id { get; set; }

        public string Name { get; set; }   

        private List<Exam>? _exams = new List<Exam>();
        
        protected ILazyLoader LazyLoader { get; set; }
        public List<Exam>? Exams
        {
            get => LazyLoader.Load(this, ref _exams);
            set => _exams = value;
        }
    }
}


using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ExamService.Models
{
    public class Exam
    {
        public int Id { get; set; }

        public String Name { get; set; } = "";

        public String AuthorEmail { get; set; } = "";

        public int TimeLimit { get; set; } = 10;

        private Topic _topic = new Topic();

        private List<Question>? _questions = new List<Question>();

        protected ILazyLoader LazyLoader { get; set; }
        public Topic Topic
        {
            get => LazyLoader.Load(this, ref _topic);
            set => _topic = value;
        }   

         public List<Question>? Questions
        {
            get => LazyLoader.Load(this, ref _questions);
            set => _questions = value;
        }   
    }
}
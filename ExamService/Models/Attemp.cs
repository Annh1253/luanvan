using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ExamService.Models
{
    public class Attemp
    {
        public int Id { get; set; }

        public double TotalScore { get; set; }
        public int TotalBonusScore { get; set; }

        public string Mode { get; set; } = "";
        public DateTime StartTime { get; set; }

        public DateTime FinishTime { get; set; }

        public int MaxCorrectStreak { get; set; } = 0;

        public Exam _exam = new Exam();

        public User _user = new User();

        public List<Answer> _answers = new List<Answer>();


        
        protected ILazyLoader LazyLoader { get; set; }


        public Exam Exam
        {
            get => LazyLoader.Load(this, ref _exam);
            set => _exam = value;
        }

         public User User
        {
            get => LazyLoader.Load(this, ref _user);
            set => _user = value;
        }

         public List<Answer> Answers
        {
            get => LazyLoader.Load(this, ref _answers);
            set => _answers = value;
        }
    }
}
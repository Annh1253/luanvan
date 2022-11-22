using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ExamService.Models
{
    public class Question
    {
        public int Id { get; set; }

        public string Content { get; set; }

        public double Score { get; set; } = 10;

        public int TimeLimit { get; set; }
        public Exam _exam = new Exam();

        private List<Option>? _options = new List<Option>();

        public List<Option>? Options
        {
            get => LazyLoader.Load(this, ref _options);
            set => _options = value;
        }

        public Exam Exam
        {
            get => LazyLoader.Load(this, ref _exam);
            set => _exam = value;
        }

        protected ILazyLoader LazyLoader { get; set; }
    }
}
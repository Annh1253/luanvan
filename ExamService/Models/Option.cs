using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ExamService.Models
{
    public class Option
    {
        public int Id { get; set; }

        public String Content { get; set; }

        public bool IsCorrect { get; set; } = false;

        private Question _question = new Question();

         public Question Question
        {
            get => LazyLoader.Load(this, ref _question);
            set => _question = value;
        }  
        protected ILazyLoader LazyLoader { get; set; }
    }
}
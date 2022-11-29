using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ExamService.Models
{
    public class Answer
    {
        public int Id { get; set; }

        public float TotalTime { get; set; } = 0;
        public int Bonus { get; set; }
        protected ILazyLoader LazyLoader { get; set; }

        public Attemp _attemp = new Attemp();

        public Attemp Attemp
        {
            get => LazyLoader.Load(this, ref _attemp);
            set => _attemp = value;
        }

        public Question _question = new Question();

        public Question Question
        {
            get => LazyLoader.Load(this, ref _question);
            set => _question = value;
        }

        public Option? _option = new Option();

        public Option? Option
        {
            get => LazyLoader.Load(this, ref _option);
            set => _option = value;
        }
    }
}
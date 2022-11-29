using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos.Answer
{
    public class AnswerRequestDto
    {
        public int QuestionId { get; set;}
        public int OptionId { get; set;}
        public float TotalTime { get; set; }
        public int Bonus { get; set; }
    }
}
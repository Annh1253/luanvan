using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos.Answer
{
    public class AnswerResponseDto
    {
        public int Id { get; set; }
        public float TotalTime { get; set; } = 0;
        public int Bonus { get; set; }
        public QuestionResponseDto Question { get; set; }
        public OptionResponseDto? Option { get; set; }

    }
}
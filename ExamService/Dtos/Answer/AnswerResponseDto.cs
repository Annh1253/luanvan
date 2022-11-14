using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos.Answer
{
    public class AnswerResponseDto
    {
        public QuestionResponseDto Question { get; set; }
        public OptionResponseDto Option { get; set; }
        public AttempResponseDto Attemp { get; set; }

    }
}
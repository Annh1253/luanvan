using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Dtos.Answer;

namespace ExamService.Dtos
{
    public class AttempResponseDto
    {
        public int Id { get; set; }
        public double TotalScore { get; set;}

        public ExamResponseDto Exam { get; set; }

        public UserResponseDto User { get; set; }

        public List<AnswerResponseDto> Answers { get; set; }
    }
}
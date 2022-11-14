using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Dtos.Answer;

namespace ExamService.Dtos
{
    public class AttempRequestDto
    {
        public double TotalScore { get; set;}
        public int ExamId { get; set; }
        public string Email {get; set;}

        public List<AnswerRequestDto> Answers { get; set; }
    }
}
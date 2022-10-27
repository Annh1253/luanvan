using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos
{
    public class AttempExamResponseDto
    {
        public double TotalScore { get; set;}

        public ExamResponseDto Exam { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos
{
    public class AttempResponseDto
    {
        public double TotalScore { get; set;}

        public ExamResponseDto Exam { get; set; }

        public UserResponseDto User { get; set; }
    }
}
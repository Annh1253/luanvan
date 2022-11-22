using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos
{
    public class QuestionResponseDto
    {
        public int Id { get; set; }

        public string Content { get; set; }
        public double Score { get; set; } = 10;
        public int TimeLimit { get; set; }
        public List<OptionResponseDto> Options { get; set; }
    }
}
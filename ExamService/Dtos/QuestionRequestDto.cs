using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos
{
    public class QuestionRequestDto
    {
        public string Content { get; set; }

        public double Score { get; set; } = 10;
        public int TimeLimit { get; set; } = 5;

        public List<OptionRequestDto> Options { get; set; }

    }
}
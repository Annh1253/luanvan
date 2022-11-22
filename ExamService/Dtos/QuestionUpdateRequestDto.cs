using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos
{
    public class QuestionUpdateRequestDto
    {
        public string? Content { get; set; }

        public double? Score { get; set; }
        public int? TimeLimit { get; set; }

        public List<OptionUpdateRequestDto>? options { get; set; }
    }
}
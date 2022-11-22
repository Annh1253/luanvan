using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos
{
    public class QuestionPublishedDto
    {
        public int ExternalId { get; set; }
        public int ExternalExamId { get; set; } = default;
        public int ExternalCorrectAnswerId { get; set; }
        public int TimeLimit { get; set; }
        public double Score { get; set; } = 10;

        public string Event { get; set; }

    }
}
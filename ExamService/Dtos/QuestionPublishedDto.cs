using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos
{
    public class QuestionPublishedDto
    {
        public int ExternalId { get; set; }
        public int ExternalExamId { get; set; }
        public int ExternalCorrectAnswerId { get; set; }

        public string Event  { get; set; }

    }
}
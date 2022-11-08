using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos
{
    public class OptionPublishedDto
    {
        public int ExternalId { get; set; }
        public int ExternalQuestionId { get; set; }
      
        public string Event  { get; set; }
    }
}
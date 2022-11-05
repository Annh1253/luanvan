using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos
{
    public class OptionPublishedDto
    {
        public int ExternalId { get; set; }
        public bool IsCorrect { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos
{
    public class ExamRequestDto
    {
        public String Name { get; set; }

        public String AuthorEmail { get; set; }

        public int TimeLimit { get; set; }

        public List<QuestionRequestDto>? Questions {get; set;}
        
    }
}
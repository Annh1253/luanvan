using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos
{
    public class ExamResponseDto
    {
        public int Id { get; set; }

        public String Name { get; set; }

        public String AuthorEmail { get; set; }

        public String Topic { get; set; }

        public int TimeLimit { get; set; }


        public List<QuestionResponseDto> Questions { get; set; }
    }
}
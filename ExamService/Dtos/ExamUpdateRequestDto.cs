using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos
{
    public class ExamUpdateRequestDto
    {
        public String? Name { get; set; }

        public String? AuthorEmail { get; set; }

         public int? TimeLimit { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos
{
    public class OptionRequestDto
    {
        public String Content { get; set; }

        public bool IsCorrect { get; set; } = false;
    }
}
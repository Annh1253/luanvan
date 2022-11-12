using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos
{
    public class ExamFilterDto
    {
        
        public String? name { get; set; }
        public String? authorEmail { get; set; }
        public bool hasQuestion { get; set; } = true;
    }
}
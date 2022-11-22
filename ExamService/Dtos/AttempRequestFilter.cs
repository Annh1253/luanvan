using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos
{
    public class AttempRequestFilter
    {
        public string? Email { get; set; }
        public int? ExamId { get; set; }
    }
}
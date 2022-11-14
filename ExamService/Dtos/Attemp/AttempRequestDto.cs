using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos.Attemp
{
    public class AttempRequestDto
    {
        public double Score { get; set; }
        public string ExamId { get; set; }
        public string Email {get; set;}
    }
}
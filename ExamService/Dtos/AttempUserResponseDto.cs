using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos
{
    public class AttempUserResponseDto
    {
        public double TotalScore { get; set;}

        public UserRequestDto User { get; set; }
    }
}
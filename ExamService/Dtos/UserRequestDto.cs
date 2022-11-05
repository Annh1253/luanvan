using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos
{
    public class UserRequestDto
    {
        public int ExternalId { get; set; } = 0;

        public string Email { get; set; } 
    }
}
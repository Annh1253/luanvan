using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos
{
    public class UserResponseDto
    {
        public int ExternalId { get; set; }

        public string Email { get; set; }
    }
}
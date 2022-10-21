using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dtos
{
    public class UserPublishedDto
    {
        public int ExternalId { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public string Event  { get; set; }

        public List<RoleDtoRequest>  Roles { get; set;}
    }
}
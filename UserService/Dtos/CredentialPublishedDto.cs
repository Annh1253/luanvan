using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Dtos
{
    public class CredentialPublishedDto
    {
        public string Email { get; set; }

        public string Password {get; set;}

        public List<string> Roles {get; set;}

    }
}
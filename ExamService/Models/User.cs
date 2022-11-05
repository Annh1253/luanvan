using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace ExamService.Models
{
    public class User
    {
        public int Id { get; set; }

        public int ExternalId { get; set; } = 0;

        public string Email { get; set; }      

        public List<Attemp>? _attemps = new List<Attemp>();

        
        protected ILazyLoader LazyLoader { get; set; }


        public List<Attemp>? Attemps
        {
            get => LazyLoader.Load(this, ref _attemps);
            set => _attemps = value;
        }
    }
}
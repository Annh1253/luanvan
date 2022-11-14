using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ExamService.Models;

namespace ExamService.Data
{
    
    public class DBContext : DbContext
    {
            
        public DBContext(DbContextOptions<DBContext> options) : base(options)
        {
            
        }


        public DbSet<Topic> Topics { get; set; }
    
        public DbSet<Question> Questions { get; set; }
        public DbSet<Option> Options { get; set; }

        public DbSet<Attemp> Attemps { get; set; }

        public DbSet<Exam> Exams { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Answer> Answers { get; set; }

      
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Contracts.RepositoryContracts;
using ExamService.Models;
using ExamService.Data;

namespace ExamService.Repositories
{
    public class TopicRepository : ITopicRepository
    {
        
        private readonly DBContext _dbContext;

        public TopicRepository(DBContext dbContext)
        {
            this._dbContext = dbContext;
            
        }
        public bool AddTopic(Topic topic)
        {
             _dbContext.Topics.Add(topic);
            return SaveChanges();
        }

        public bool Exist(int id)
        {
            return _dbContext.Topics.Any(r => r.Id == id);
        }

        public bool ExistByName(string name)
        {
            return _dbContext.Topics.Any(r => r.Name == name);
        }

        public List<Topic> GetAllTopics()
        {
            return _dbContext.Topics.ToList();
        }

        public Topic GetById(int id)
        {
            return _dbContext.Topics.FirstOrDefault(r => r.Id ==id);
        }

        public Topic GetByName(string name)
        {
            return _dbContext.Topics.FirstOrDefault(r => r.Name == name);
        }

        private bool SaveChanges()
        {
            return _dbContext.SaveChanges() >=0;
        }
    }
}
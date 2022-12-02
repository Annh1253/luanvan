using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Contracts.RepositoryContracts;
using ExamService.Models;
using ExamService.Data;
using ExamService.Dtos;
using ExamService.Helpers;

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

        public Topic DeleteTopic(Topic topicToDelete)
        {
            Topic deletedTopic = new Topic{
                Name = topicToDelete.Name
            };
              try{
                _dbContext.Topics.Remove(topicToDelete);
            }catch(Exception ex)
            {
                throw ex;
            }
            if(SaveChanges())
                return deletedTopic;
            return null;
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

        public Topic UpdateTopic(int oldTopicId, TopicRequestDto topicRequestDto)
        {
               try{
                 Topic oldTopic = _dbContext.Topics.First(e => e.Id == oldTopicId);
                CRUDHelper.CopyNoneNull(topicRequestDto, oldTopic);
                if(SaveChanges())
                    return oldTopic;
                return null;
            }catch(Exception ex){
                throw ex;   
            }
        }

        private bool SaveChanges()
        {
            return _dbContext.SaveChanges() >=0;
        }
    }
}
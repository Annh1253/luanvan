using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Dtos;
using ExamService.Models;
using ExamService.Response;

namespace ExamService.Contracts.RepositoryContracts
{
    public interface ITopicRepository
    {
        List<Topic> GetAllTopics();

        Topic GetById(int id);

        bool AddTopic(Topic topic);

        Topic UpdateTopic(int oldTopicId, TopicRequestDto topicRequestDto);
        Topic DeleteTopic(Topic topicToDelete);
        Topic GetByName(string name);
      
        bool Exist(int id);
        bool ExistByName(string name);
    }
}
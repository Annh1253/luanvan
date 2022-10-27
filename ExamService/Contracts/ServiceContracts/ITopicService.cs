using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Dtos;
using ExamService.Response;

namespace ExamService.Contracts.ServiceContracts
{
    public interface ITopicService
    {
        ServiceResponse<List<TopicResponseDto>> GetTopics();
        ServiceResponse<TopicResponseDto> GetById(int id);
 
        ServiceResponse<TopicResponseDto> AddTopic(TopicRequestDto topicRequestDto);

        bool Exist(int id);
    }
}
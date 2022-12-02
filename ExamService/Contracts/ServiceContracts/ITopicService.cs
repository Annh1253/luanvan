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
        ServiceResponse<TopicResponseDto> UpdateTopic(int oldTopicId , TopicRequestDto updateTopic);
        ServiceResponse<TopicResponseDto> DeleteTopic(int topicId);
        bool Exist(int id);
    }
}
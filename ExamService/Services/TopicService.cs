
using System.Net;
using AutoMapper;
using ExamService.Contracts.RepositoryContracts;
using ExamService.Contracts.ServiceContracts;
using ExamService.Dtos;
using ExamService.Models;
using ExamService.Response;


namespace ExamService.Services
{
    public class TopicService : ITopicService
    {
        private readonly ITopicRepository _topicRepository;
        private readonly IMapper _mapper;

        public TopicService(ITopicRepository topicRepository, IMapper mapper)
        {
            this._topicRepository = topicRepository;
            this._mapper = mapper;
        }
        public ServiceResponse<TopicResponseDto> AddTopic(TopicRequestDto topic)
        {
            Topic topicEntity = _mapper.Map<Topic>(topic);
            bool SavedSucceeded = _topicRepository.AddTopic(topicEntity);   
            
            // if guard
            if (!SavedSucceeded) return new ServiceResponse<TopicResponseDto>(){
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "Some thing went wrong when saving"
            };
            
            TopicResponseDto topicReadDto = _mapper.Map<TopicResponseDto>(topicEntity);
            var serviceResponse = new ServiceResponse<TopicResponseDto>();
            serviceResponse.Success = true;
            serviceResponse.Data = topicReadDto;
            return serviceResponse;
        }

        public bool Exist(int id)
        {
           return _topicRepository.Exist(id);
        }

        public ServiceResponse<TopicResponseDto> GetById(int id)
        {
            Topic topicEntity = _topicRepository.GetById(id);
         
            
            // if guard
            if (topicEntity != null) return new ServiceResponse<TopicResponseDto>(){
                StatusCode = HttpStatusCode.NotFound,
                Message = "Topic not found"
            };
            
            TopicResponseDto topicReadDto = _mapper.Map<TopicResponseDto>(topicEntity);
            var serviceResponse = new ServiceResponse<TopicResponseDto>();
            serviceResponse.Success = true;
            serviceResponse.Data = topicReadDto;
            return serviceResponse;
        }

        public ServiceResponse<List<TopicResponseDto>> GetTopics()
        {
            List<Topic> topicList = _topicRepository.GetAllTopics();
            List<TopicResponseDto> topicReadDtoList = _mapper.Map<List<TopicResponseDto>>(topicList);
            var serviceResponse = new ServiceResponse<List<TopicResponseDto>>();
            serviceResponse.Success = true;
            serviceResponse.Data = topicReadDtoList;
            return serviceResponse;
        }
    }
}
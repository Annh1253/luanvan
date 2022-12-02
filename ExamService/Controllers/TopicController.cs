using AutoMapper;
using ExamService.Contracts.ServiceContracts;
using ExamService.Dtos;
using ExamService.Response;
using Microsoft.AspNetCore.Mvc;

namespace ExamService.Controllers
{
    [ApiController]
    [Route("api/examservice/[controller]")]
    public class TopicController : ControllerBase
    {
        private readonly ITopicService _topicService;
        private readonly IMapper _mapper;

        public TopicController(ITopicService topicService, IMapper mapper)
        {
            this._topicService = topicService;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetTopics()
        {
            ServiceResponse<List<TopicResponseDto>> serviceResponse = _topicService.GetTopics();
            ControllerResponse<List<TopicResponseDto>> controllerResponse = _mapper.Map<ControllerResponse<List<TopicResponseDto>>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }

        [HttpPost]
        public IActionResult AddTopic(TopicRequestDto newTopic)
        {
            ServiceResponse<TopicResponseDto> serviceResponse = _topicService.AddTopic(newTopic);
            ControllerResponse<TopicResponseDto> controllerResponse = _mapper.Map<ControllerResponse<TopicResponseDto>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }

        [HttpPatch("{oldTopicId}")]
        public IActionResult UpdateTopic(int oldTopicId, TopicRequestDto updateTopic)
        {
            ServiceResponse<TopicResponseDto> serviceResponse = _topicService.UpdateTopic(oldTopicId, updateTopic);
            ControllerResponse<TopicResponseDto> controllerResponse = _mapper.Map<ControllerResponse<TopicResponseDto>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }

        [HttpDelete("{topicToDeleteId}")]
        public IActionResult UpdateTopic(int topicToDeleteId)
        {
            ServiceResponse<TopicResponseDto> serviceResponse = _topicService.DeleteTopic(topicToDeleteId);
            ControllerResponse<TopicResponseDto> controllerResponse = _mapper.Map<ControllerResponse<TopicResponseDto>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }
    }
}
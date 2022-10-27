using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExamService.Contracts.ServiceContracts;
using ExamService.Dtos;
using ExamService.Response;
using Microsoft.AspNetCore.Mvc;

namespace ExamService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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
    }
}
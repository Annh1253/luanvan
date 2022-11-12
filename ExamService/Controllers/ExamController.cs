using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExamService.Contracts.ServiceContracts;
using ExamService.Dtos;
using ExamService.Filters;
using ExamService.Response;
using Microsoft.AspNetCore.Mvc;

namespace ExamService.Controllers
{

    [ApiController]
    [Route("api/examservice/[controller]")]
    // [AuthenticateFilter]
    public class ExamController : ControllerBase
    {
        private readonly IExamService _examService;
        private readonly IMapper _mapper;

        public ExamController(IExamService examService, IMapper mapper)
        {
            this._examService = examService;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetExams([FromQuery] ExamFilterDto examFilter)
        {
            Console.WriteLine(examFilter.hasQuestion);
            ServiceResponse<List<ExamResponseDto>> serviceResponse = _examService.GetExams(examFilter);
            ControllerResponse<List<ExamResponseDto>> controllerResponse = _mapper.Map<ControllerResponse<List<ExamResponseDto>>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }

         [HttpGet("{examId}")]
        public IActionResult GetExamById(int examId)
        {
            ServiceResponse<ExamResponseDto> serviceResponse = _examService.GetById(examId);
            ControllerResponse<ExamResponseDto> controllerResponse = _mapper.Map<ControllerResponse<ExamResponseDto>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }

        [HttpGet("topic/{topicName}")]
        public IActionResult GetExamsByTopic(string topicName)
        {
            ServiceResponse<List<ExamResponseDto>> serviceResponse = _examService.GetExamsByTopicName(topicName);
            ControllerResponse<List<ExamResponseDto>> controllerResponse = _mapper.Map<ControllerResponse<List<ExamResponseDto>>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }

        [HttpGet("author/{authorEmail}")]
        public IActionResult GetExamsByAuthor(string authorEmail)
        {
            ServiceResponse<List<ExamResponseDto>> serviceResponse = _examService.GetExamsByAuthorEmail(authorEmail);
            ControllerResponse<List<ExamResponseDto>> controllerResponse = _mapper.Map<ControllerResponse<List<ExamResponseDto>>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }


        [HttpPost("topic/{topicId}")]
        public IActionResult AddExam(int topicId,[FromBody] ExamRequestDto examRequestDto)
        {
            ServiceResponse<ExamResponseDto> serviceResponse = _examService.AddExam(topicId, examRequestDto);
            ControllerResponse<ExamResponseDto> controllerResponse = _mapper.Map<ControllerResponse<ExamResponseDto>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }


        [HttpPatch("{examId}")]
         public IActionResult UpdateExam(int examId, ExamUpdateRequestDto examRequestDto)
        {
            ServiceResponse<ExamResponseDto> serviceResponse = _examService.UpdateExam(examId, examRequestDto);
            ControllerResponse<ExamResponseDto> controllerResponse = _mapper.Map<ControllerResponse<ExamResponseDto>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        } 
        

        [HttpDelete("{examId}")]
        public IActionResult RemoveExam(int examId)
        {
            ServiceResponse<ExamResponseDto> serviceResponse = _examService.RemoveExam(examId);
            ControllerResponse<ExamResponseDto> controllerResponse = _mapper.Map<ControllerResponse<ExamResponseDto>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }
    }
}
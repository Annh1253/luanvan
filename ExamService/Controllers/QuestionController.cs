using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExamService.Contracts.ServiceContracts;
using ExamService.Dtos;
using ExamService.Filters;
using ExamService.Response;
using ExamService.Services;
using Microsoft.AspNetCore.Mvc;

namespace ExamService.Controllers
{
    [ApiController]
    [Route("api/examservice/[controller]")]
    // [AuthenticateFilter]

    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;

        public QuestionController(IQuestionService questionService, IMapper mapper)
        {
            this._questionService = questionService;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetQuestions()
        {
            ServiceResponse<List<QuestionResponseDto>> serviceResponse = _questionService.GetQuestions();
            ControllerResponse<List<QuestionResponseDto>> controllerResponse = _mapper.Map<ControllerResponse<List<QuestionResponseDto>>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }

        [HttpGet("{questionId}")]
        public IActionResult GetQuestionById(int questionId)
        {
            ServiceResponse<QuestionResponseDto> serviceResponse = _questionService.GetById(questionId);
            ControllerResponse<QuestionResponseDto> controllerResponse = _mapper.Map<ControllerResponse<QuestionResponseDto>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }

       

        [HttpPost("exam/{examId}")]
        public IActionResult AddQuestion(int examId, QuestionRequestDto questionRequestDto)
        {
            ServiceResponse<QuestionResponseDto> serviceResponse = _questionService.AddQuestion(examId, questionRequestDto);
            ControllerResponse<QuestionResponseDto> controllerResponse = _mapper.Map<ControllerResponse<QuestionResponseDto>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }


        [HttpPatch("{questionId}")]
         public IActionResult UpdateQuestion(int questionId, QuestionUpdateRequestDto questionRequestDto)
        {
            ServiceResponse<QuestionResponseDto> serviceResponse = _questionService.UpdateQuestion(questionId, questionRequestDto);
            ControllerResponse<QuestionResponseDto> controllerResponse = _mapper.Map<ControllerResponse<QuestionResponseDto>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        } 
        

        [HttpDelete("{questionId}")]
        public IActionResult RemoveQuestion(int questionId)
        {
            ServiceResponse<QuestionResponseDto> serviceResponse = _questionService.RemoveQuestion(questionId);
            ControllerResponse<QuestionResponseDto> controllerResponse = _mapper.Map<ControllerResponse<QuestionResponseDto>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }
    }
}
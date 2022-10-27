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
        public IActionResult GetExams()
        {
            ServiceResponse<List<ExamResponseDto>> serviceResponse = _examService.GetExams();
            ControllerResponse<List<ExamResponseDto>> controllerResponse = _mapper.Map<ControllerResponse<List<ExamResponseDto>>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }

        [HttpPost("topic/{topicId}")]
        public IActionResult AddExam(int topicId, ExamRequestDto examRequestDto)
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
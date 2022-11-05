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

    public class OptionController : ControllerBase
    {
         private readonly IOptionService _optionService;
        private readonly IMapper _mapper;

        public OptionController(IOptionService optionService, IMapper mapper)
        {
            this._optionService = optionService;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetOptions()
        {
            
            ServiceResponse<List<OptionResponseDto>> serviceResponse = _optionService.GetOptions();
            ControllerResponse<List<OptionResponseDto>> controllerResponse = _mapper.Map<ControllerResponse<List<OptionResponseDto>>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }

       

        [HttpPost("question/{questionId}")]
        public IActionResult AddOption(int questionId, OptionRequestDto optionRequestDto)
        {
            ServiceResponse<OptionResponseDto> serviceResponse = _optionService.AddOption(questionId, optionRequestDto);
            ControllerResponse<OptionResponseDto> controllerResponse = _mapper.Map<ControllerResponse<OptionResponseDto>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }


        [HttpPatch("{optionId}")]
         public IActionResult UpdateOption(int optionId, OptionUpdateRequestDto optionRequestDto)
        {
            ServiceResponse<OptionResponseDto> serviceResponse = _optionService.UpdateOption(optionId, optionRequestDto);
            ControllerResponse<OptionResponseDto> controllerResponse = _mapper.Map<ControllerResponse<OptionResponseDto>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        } 
        

        [HttpDelete("{optionId}")]
        public IActionResult RemoveOption(int optionId)
        {
            ServiceResponse<OptionResponseDto> serviceResponse = _optionService.RemoveOption(optionId);
            ControllerResponse<OptionResponseDto> controllerResponse = _mapper.Map<ControllerResponse<OptionResponseDto>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }
    }
}
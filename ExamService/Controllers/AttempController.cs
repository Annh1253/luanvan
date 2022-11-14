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
    [Route("api/examservice/[controller]")]
    public class AttempController : ControllerBase
    {
        private readonly IAttempService _attempService;
        private readonly IMapper _mapper;

        public AttempController(IAttempService attempService, IMapper mapper)
        {
            this._attempService = attempService;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll(){
            ServiceResponse<List<AttempResponseDto>> serviceResponse = _attempService.GetAttemps();
            ControllerResponse<List<AttempResponseDto>> controllerResponse = _mapper.Map<ControllerResponse<List<AttempResponseDto>>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }
    }
}
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            this._userService = userService;
            this._mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetUsers()
        {
            ServiceResponse<List<UserResponseDto>> serviceResponse = _userService.GetUsers();
            ControllerResponse<List<UserResponseDto>> controllerResponse = _mapper.Map<ControllerResponse<List<UserResponseDto>>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerResponse);
        }
    }
}
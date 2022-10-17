using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.Contracts.InterfaceContracts;
using UserService.Dtos;
using UserService.Response;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            this._mapper = mapper;
            this._userService = userService;
            
        }
        [HttpGet]
        public ActionResult GetUsers()
        {
            ServiceResponse<List<UserDtoResponse>> serviceResponse = _userService.GetUsers();
            ControllerResponse<List<UserDtoResponse>> controllerReponse = _mapper.Map<ControllerResponse<List<UserDtoResponse>>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerReponse);
        }

        [HttpPost("{RoleId}")]
        public IActionResult AddUser(int RoleId, UserDtoRequest userCreateDto)
        {
            ServiceResponse<UserDtoResponse> serviceResponse = _userService.AddUser(RoleId, userCreateDto);
            ControllerResponse<UserDtoResponse> controllerReponse = _mapper.Map<ControllerResponse<UserDtoResponse>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerReponse);
        }



    }
}
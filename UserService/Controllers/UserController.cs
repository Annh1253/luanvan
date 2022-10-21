using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.AsyncDataServices;
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
        private readonly IMessageBusClient _messageBusClient;

        public UserController(IUserService userService, IMapper mapper, IMessageBusClient _messageBusClient)
        {
            this._mapper = mapper;
            this._messageBusClient = _messageBusClient;
            this._userService = userService;
            
        }

        [Authorize(Roles ="Admin")]
        [HttpGet]
        public ActionResult GetUsers()
        {
            ServiceResponse<List<UserDtoResponse>> serviceResponse = _userService.GetUsers();
            ControllerResponse<List<UserDtoResponse>> controllerReponse = _mapper.Map<ControllerResponse<List<UserDtoResponse>>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerReponse);
        }

        [HttpPost("/role/{RoleId}")]
        public IActionResult AddUser(int RoleId, UserDtoRequest userCreateDto)
        {
            ServiceResponse<UserDtoResponse> serviceResponse = _userService.AddUser(RoleId, userCreateDto);
            ControllerResponse<UserDtoResponse> controllerReponse = _mapper.Map<ControllerResponse<UserDtoResponse>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerReponse);
        }

        [HttpPatch("{UserId}")]
        public IActionResult UpdateUser(int UserId, UserDtoRequest userDtoRequest)
        {
            ServiceResponse<UserDtoResponse> serviceResponse = _userService.UpdateUser(UserId, userDtoRequest);
            ControllerResponse<UserDtoResponse> controllerReponse = _mapper.Map<ControllerResponse<UserDtoResponse>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerReponse);
        }

        [HttpDelete("UserId")]
        public IActionResult DeleteUser(int UserId)
        {
            ServiceResponse<UserDtoResponse> serviceResponse = _userService.DeleteUser(UserId);
            ControllerResponse<UserDtoResponse> controllerReponse = _mapper.Map<ControllerResponse<UserDtoResponse>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerReponse);
        }


    }
}
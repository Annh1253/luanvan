using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;

using Microsoft.AspNetCore.Mvc;
using UserService.AsyncDataServices;
using UserService.Constants;
using UserService.Contracts.InterfaceContracts;
using UserService.Dtos;
using UserService.Filters;
using UserService.Helpers;
using UserService.Response;

namespace UserService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AuthenticateFilter]
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

      
        [HttpGet]
        // [AuthorizeFilter(RoleType.Contestant)]
        public ActionResult GetUsers()
        {

            ServiceResponse<List<UserDtoResponse>> serviceResponse = _userService.GetUsers();
            ControllerResponse<List<UserDtoResponse>> controllerReponse = _mapper.Map<ControllerResponse<List<UserDtoResponse>>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerReponse);
        }

        [HttpGet("{userId}")]
        public ActionResult GetUserById(int userId)
        {
            ServiceResponse<UserDtoResponse> serviceResponse = _userService.GetById(userId);
            ControllerResponse<UserDtoResponse> controllerReponse = _mapper.Map<ControllerResponse<UserDtoResponse>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerReponse);
        }

        [HttpGet("email/{userEmail}")]
        public ActionResult GetUserByEmail(string userEmail)
        {
            ServiceResponse<UserDtoResponse> serviceResponse = _userService.GetByEmail(userEmail);
            ControllerResponse<UserDtoResponse> controllerReponse = _mapper.Map<ControllerResponse<UserDtoResponse>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerReponse);
        }

        [HttpPost("role/{RoleId}")]
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

        [HttpDelete("{UserId}")]
        public IActionResult DeleteUser(int UserId)
        {
            ServiceResponse<UserDtoResponse> serviceResponse = _userService.DeleteUser(UserId);
            ControllerResponse<UserDtoResponse> controllerReponse = _mapper.Map<ControllerResponse<UserDtoResponse>>(serviceResponse);
            return StatusCode((int)serviceResponse.StatusCode, controllerReponse);
        }


    }
}
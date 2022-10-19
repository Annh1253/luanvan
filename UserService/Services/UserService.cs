using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using UserService.AsyncDataServices;
using UserService.Contracts.InterfaceContracts;
using UserService.Contracts.RepositoryContracts;
using UserService.Dtos;
using UserService.Models;
using UserService.Response;

namespace UserService.Services
{
    public class UsersService : IUserService
    {
       private readonly IUserRepository _userRepository;
        private readonly IMapper _modelMapper;
        private readonly ILogger<UsersService> _logger;
        private readonly IMessageBusClient _messageBusClient;
        private readonly IRoleRepository _roleRepository;

        public UsersService(IUserRepository _userRepository, IRoleRepository _roleRepository, IMapper mapper, ILogger<UsersService> logger, IMessageBusClient _messageBusClient)
        {
            this._roleRepository = _roleRepository;
            this._userRepository = _userRepository;
            this._modelMapper = mapper;
            this._logger = logger;
            this._messageBusClient = _messageBusClient;
        }
        public ServiceResponse<UserDtoResponse> AddUser(int RoleId, UserDtoRequest user)
        {
            User userEntity = _modelMapper.Map<User>(user);
            Role role = _roleRepository.GetById(RoleId);
            userEntity.Roles.Add(role);
            bool SavedSucceeded = _userRepository.AddUser(userEntity);   
            
            // if guard
            if (!SavedSucceeded) return new ServiceResponse<UserDtoResponse>(){
                Success = false,
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "Some thing went wrong when saving"
            };
            
            UserDtoResponse userReadDto = _modelMapper.Map<UserDtoResponse>(userEntity);
            var serviceResponse = new ServiceResponse<UserDtoResponse>();
            serviceResponse.Data = userReadDto;
          
            //Publish user to message bus
            var userPublishedDto = _modelMapper.Map<UserPublishedDto>(userEntity);
            userPublishedDto.Event = "NewUserCreate";
            _messageBusClient.PublishNewPlatform(userPublishedDto);
           
            return serviceResponse;
        }

        public bool Exist(int id)
        {
            return _userRepository.Exist(id);
        }

        public bool Exist(string email)
        {
            return _userRepository.Exist(email);
        }

        public ServiceResponse<UserDtoResponse> GetById(int id)
        {
            User user = _userRepository.GetById(id);
            var serviceResponse = new ServiceResponse<UserDtoResponse>();
            if(user == null)
            {
                serviceResponse.Data = null;
                serviceResponse.Success = false;
                serviceResponse.Message = "User not found";
                serviceResponse.StatusCode = HttpStatusCode.NotFound;
            }
            else
            {
                UserDtoResponse userReadDto = _modelMapper.Map<UserDtoResponse>(user);
                serviceResponse.Data = userReadDto;
            }
            return serviceResponse;
        }

        public ServiceResponse<List<UserDtoResponse>> GetUsers()
        {
       

            List<User> userList = (List<User>)_userRepository.GetAllUsers();
            List<UserDtoResponse> userDtoList = _modelMapper.Map<List<UserDtoResponse>>(userList);
            var serviceResponse = new ServiceResponse<List<UserDtoResponse>>()
            {
                Data = userDtoList,
            };
            return serviceResponse;
        }

        public ServiceResponse<UserDtoResponse> UpdateUser(UserDtoRequest user)
        {
            throw new NotImplementedException();
        }
    }
}
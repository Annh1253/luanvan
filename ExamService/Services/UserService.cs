using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExamService.Contracts.RepositoryContracts;
using ExamService.Contracts.ServiceContracts;
using ExamService.Dtos;
using ExamService.Models;
using ExamService.Response;

namespace ExamService.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            this._mapper = mapper;
            this._userRepository = userRepository;
        }
        public ServiceResponse<List<UserResponseDto>> GetUsers()
        {
            List<User> userList = _userRepository.GetAllUsers();
            List<UserResponseDto> userReadDtoList = _mapper.Map<List<UserResponseDto>>(userList);
            var serviceResponse = new ServiceResponse<List<UserResponseDto>>();
            serviceResponse.Data = userReadDtoList;
            serviceResponse.Success = true;
            return serviceResponse;
        }
    }
}
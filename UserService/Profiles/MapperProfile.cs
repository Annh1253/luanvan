using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UserService.Dtos;
using UserService.Models;
using UserService.Response;

namespace UserService.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<User, UserDtoResponse>();
            CreateMap<UserDtoRequest, User>();
            CreateMap<Role, RoleDtoRequest>();
            CreateMap<RoleDtoRequest, Role>();
            CreateMap<ServiceResponse<UserDtoResponse>, ControllerResponse<UserDtoResponse>>();
            CreateMap<ServiceResponse<List<UserDtoResponse>>, ControllerResponse<List<UserDtoResponse>>>();

            CreateMap<ServiceResponse<RoleDtoRequest>, ControllerResponse<RoleDtoRequest>>();
            CreateMap<ServiceResponse<List<RoleDtoRequest>>, ControllerResponse<List<RoleDtoRequest>>>();
        }
    }
}
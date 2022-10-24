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

            CreateMap<CredentialPublishedDto, UserDtoRequest>();

            CreateMap<ServiceResponse<UserDtoResponse>, ControllerResponse<UserDtoResponse>>();
            CreateMap<ServiceResponse<List<UserDtoResponse>>, ControllerResponse<List<UserDtoResponse>>>();

            CreateMap<ServiceResponse<RoleDtoRequest>, ControllerResponse<RoleDtoRequest>>();
            CreateMap<ServiceResponse<List<RoleDtoRequest>>, ControllerResponse<List<RoleDtoRequest>>>();

            CreateMap<UserDtoResponse, UserPublishedDto>();
            CreateMap<UserDtoRequest, UserPublishedDto>();
            CreateMap<User, UserPublishedDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));;
        }
    }
}
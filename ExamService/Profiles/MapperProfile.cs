using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExamService.Dtos;
using ExamService.Models;
using ExamService.Response;


namespace ExamService.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Topic, TopicResponseDto>();
            CreateMap<TopicRequestDto, Topic>();

            CreateMap<Exam, ExamResponseDto>();
            CreateMap<ExamRequestDto, Exam>();

            CreateMap<Question, QuestionResponseDto>();
            CreateMap<QuestionRequestDto, Question>();
       
            CreateMap<Option, OptionResponseDto>();
            CreateMap<OptionRequestDto, Option>();

            CreateMap<User, UserResponseDto>();
            CreateMap<UserRequestDto, User>();

            CreateMap<Attemp, AttempResponseDto>();
            CreateMap<Attemp, AttempUserResponseDto>();
            CreateMap<Attemp, AttempExamResponseDto>();
            CreateMap<AttempRequestDto, Attemp>();

            CreateMap<ServiceResponse<TopicResponseDto>, ControllerResponse<TopicResponseDto>>();
            CreateMap<ServiceResponse<List<TopicResponseDto>>, ControllerResponse<List<TopicResponseDto>>>();

            CreateMap<ServiceResponse<ExamResponseDto>, ControllerResponse<ExamResponseDto>>();
            CreateMap<ServiceResponse<List<ExamResponseDto>>, ControllerResponse<List<ExamResponseDto>>>();

           
            CreateMap<ServiceResponse<QuestionResponseDto>, ControllerResponse<QuestionResponseDto>>();
            CreateMap<ServiceResponse<List<QuestionResponseDto>>, ControllerResponse<List<QuestionResponseDto>>>();

            CreateMap<ServiceResponse<OptionResponseDto>, ControllerResponse<OptionResponseDto>>();
            CreateMap<ServiceResponse<List<OptionResponseDto>>, ControllerResponse<List<OptionResponseDto>>>();

            CreateMap<ServiceResponse<UserResponseDto>, ControllerResponse<UserResponseDto>>();
            CreateMap<ServiceResponse<List<UserResponseDto>>, ControllerResponse<List<UserResponseDto>>>();

            CreateMap<CredentialPublishedDto, UserRequestDto>();
            
            CreateMap<ExamResponseDto, ExamPublishedDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<QuestionResponseDto, QuestionPublishedDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));
            CreateMap<OptionResponseDto, OptionPublishedDto>()
                .ForMember(dest => dest.ExternalId, opt => opt.MapFrom(src => src.Id));    
        }
    }
}
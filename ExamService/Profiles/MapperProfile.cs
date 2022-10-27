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

           
            
        }
    }
}
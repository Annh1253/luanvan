using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using ExamService.Constants;
using ExamService.Contracts.RepositoryContracts;
using ExamService.Contracts.ServiceContracts;
using ExamService.Dtos;
using ExamService.Models;
using ExamService.Response;

namespace ExamService.Services
{
    public class OptionService : IOptionService
    {
        private readonly IOptionRepository _optionRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;

        public OptionService(IOptionRepository optionRepository, IQuestionRepository questionRepository, IMapper mapper)
        {
            this._optionRepository = optionRepository;
            this._questionRepository = questionRepository;
            this._mapper = mapper;
        }
        public ServiceResponse<OptionResponseDto> AddOption(int QuestionId, OptionRequestDto optionRequestDto)
        {
           if(!_questionRepository.Exist(QuestionId))
           {
                return new ServiceResponse<OptionResponseDto>()
                {
                    Message = "Question not found",
                    StatusCode = HttpStatusCode.NotFound,
                };
               
           };
            Option optionEntity = _mapper.Map<Option>(optionRequestDto);
            Question questionEntity = _questionRepository.GetById(QuestionId);
            optionEntity.Question = questionEntity;
            bool isSucceed = _optionRepository.AddOption(optionEntity);
            if(!isSucceed)
            {
                return new ServiceResponse<OptionResponseDto>()
                {
                    Message = ErrorMessage.CREATE,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
            return new ServiceResponse<OptionResponseDto>()
            {
                Success = true,
                Message = SuccessMessage.CREATE,
                StatusCode = HttpStatusCode.Created
            };
        }

        public bool Exist(int id)
        {
           return _optionRepository.Exist(id);
        }

        public ServiceResponse<OptionResponseDto> GetById(int id)
        {
           if(!_optionRepository.Exist(id))
           {
                return new ServiceResponse<OptionResponseDto>()
                {
                    Message = "Option not found",
                    StatusCode = HttpStatusCode.NotFound,
                };
           };

           Option requestedOption = _optionRepository.GetById(id);
           OptionResponseDto optionResponseDto = _mapper.Map<OptionResponseDto>(requestedOption);

           return new ServiceResponse<OptionResponseDto>()
           {
                Success = true,
                Data = optionResponseDto,
                Message = SuccessMessage.CREATE,
                StatusCode = HttpStatusCode.NotFound
           } ;
        }

        public ServiceResponse<List<OptionResponseDto>> GetOptions()
        {
            List<Option> optionList = _optionRepository.GetAllOptions();
            List<OptionResponseDto> optionResponseDtoList = _mapper.Map<List<OptionResponseDto>>(optionList);
            return new ServiceResponse<List<OptionResponseDto>>()
            {
                Data = optionResponseDtoList,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };
        }

        public ServiceResponse<OptionResponseDto> RemoveOption(int id)
        {
           if(!_optionRepository.Exist(id))
           {
                return new ServiceResponse<OptionResponseDto>()
                {
                    Message = "Option not found",
                    StatusCode = HttpStatusCode.NotFound
                };
           }
            Option optionToDelete = _optionRepository.GetById(id);
            bool isSucceed = _optionRepository.RemoveOption(optionToDelete);
            if(!isSucceed)
            {
                return new ServiceResponse<OptionResponseDto>()
                {
                    Message = ErrorMessage.DELETE,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
            return new ServiceResponse<OptionResponseDto>()
            {
                Success = true,
                Message = SuccessMessage.DELETE,
                StatusCode = HttpStatusCode.OK
            };
        }

        public ServiceResponse<OptionResponseDto> UpdateOption(int oldOptionId, OptionUpdateRequestDto OptionRequestDto)
        {
            if(!_optionRepository.Exist(oldOptionId))
           {
                return new ServiceResponse<OptionResponseDto>()
                {
                    Message = "Option not found",
                    StatusCode = HttpStatusCode.NotFound
                };
           }
           
            bool isSucceed = _optionRepository.UpdateOption(oldOptionId, OptionRequestDto);
            if(!isSucceed)
            {
                return new ServiceResponse<OptionResponseDto>()
                {
                    Message = ErrorMessage.UPDATE,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
            return new ServiceResponse<OptionResponseDto>()
            {
                Success = true,
                Message = SuccessMessage.UPDATE,
                StatusCode = HttpStatusCode.OK
            };
        }
    }
}
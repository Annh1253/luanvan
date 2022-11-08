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
using ExamService.Helpers;
using ExamService.Models;
using ExamService.Response;
using ExamService.Ultils;

namespace ExamService.Services
{
    public class OptionService : IOptionService
    {
        private readonly IOptionRepository _optionRepository;
        private readonly IQuestionRepository _questionRepository;
        private readonly IMapper _mapper;
        private readonly IMessagePublisher _messagePublisher;

        public OptionService(IOptionRepository optionRepository, IQuestionRepository questionRepository, IMapper mapper, IMessagePublisher messagePublisher)
        {
            this._optionRepository = optionRepository;
            this._questionRepository = questionRepository;
            this._mapper = mapper;
            this._messagePublisher = messagePublisher;
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
            try{
                bool isChanged = _optionRepository.AddOption(optionEntity);
                if(!isChanged)
                {
                    
                    return new ServiceResponse<OptionResponseDto>()
                    {
                        Message = SuccessMessage.UNCHANGE,
                        StatusCode = HttpStatusCode.OK
                    };
                }
            }catch(Exception ex){
                return new ServiceResponse<OptionResponseDto>()
                    {
                        Message = ex.Message,
                        StatusCode = HttpStatusCode.InternalServerError
                    };
            }
            if(optionRequestDto.IsCorrect)
            {
                Question question = _questionRepository.GetById(QuestionId);
                QuestionResponseDto questionResponseDto = _mapper.Map<QuestionResponseDto>(question);
                _messagePublisher.PublishQuestion(questionResponseDto, EventType.NewOptionCreate);
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
            Option optionHolder = new Option(){
                Id = optionToDelete.Id,
                Question = optionToDelete.Question,
                IsCorrect = optionToDelete.IsCorrect
            };
            bool isSucceed = _optionRepository.RemoveOption(optionToDelete);
            if(!isSucceed)
            {
                return new ServiceResponse<OptionResponseDto>()
                {
                    Message = ErrorMessage.DELETE,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
            if(optionHolder.IsCorrect)
            {
                Question question = optionHolder.Question;
                QuestionResponseDto questionResponseDto = _mapper.Map<QuestionResponseDto>(question);
                _messagePublisher.PublishQuestion(questionResponseDto, EventType.DeleteOption);
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
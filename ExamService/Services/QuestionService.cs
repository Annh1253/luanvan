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
using ExamService.Data;
using ExamService.Helpers;
using ExamService.Ultils;

namespace ExamService.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly DBContext _dbContext;
        private readonly IQuestionRepository _questionRepository;
        private readonly IExamRepository _examRepository;
        private readonly IOptionRepository _optionRepository;
        private readonly IMapper _mapper;
        private readonly IMessagePublisher _messagePublisher;

        public QuestionService(DBContext dbContext, IQuestionRepository questionRepository, IExamRepository examRepository, IOptionRepository optionRepository, IMapper mapper, IMessagePublisher messagePublisher)
        {
            this._dbContext = dbContext;
            this._questionRepository = questionRepository;
            this._examRepository = examRepository;
            this._optionRepository = optionRepository;
            this._mapper = mapper;
            this._messagePublisher = messagePublisher;
        }

        public ServiceResponse<QuestionResponseDto> AddQuestion(int ExamId, QuestionRequestDto questionRequestDto)
        {
            if (!_examRepository.Exist(ExamId))
            {
                return new ServiceResponse<QuestionResponseDto>()
                {
                    Message = "Exam not exist",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            Exam examEntity = _examRepository.GetById(ExamId);
            Question questionEntity = _mapper.Map<Question>(questionRequestDto);
            questionEntity.Exam = examEntity;
            bool isSuceeded = _questionRepository.AddQuestion(questionEntity);
            if (!isSuceeded)
            {
                return new ServiceResponse<QuestionResponseDto>()
                {
                    Message = "Question not exist",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

            QuestionResponseDto questionResponseDto = _mapper.Map<QuestionResponseDto>(questionEntity);

            _messagePublisher.PublishQuestion(questionResponseDto, ExamId, EventType.NewQuestionCreate);

            return new ServiceResponse<QuestionResponseDto>()
            {
                Data = questionResponseDto,
                StatusCode = HttpStatusCode.OK,
                Message = SuccessMessage.CREATE
            };
        }

        public bool Exist(int id)
        {
            return _questionRepository.Exist(id);
        }

        public ServiceResponse<QuestionResponseDto> GetById(int id)
        {
            Question questionEntity = _questionRepository.GetById(id);


            // if guard
            if (questionEntity == null) return new ServiceResponse<QuestionResponseDto>()
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = "Question not found"
            };

            QuestionResponseDto questionReadDto = _mapper.Map<QuestionResponseDto>(questionEntity);
            var serviceResponse = new ServiceResponse<QuestionResponseDto>();
            serviceResponse.Data = questionReadDto;
            return serviceResponse;
        }

        public ServiceResponse<List<QuestionResponseDto>> GetQuestions()
        {
            List<Question> questionList = _questionRepository.GetAllQuestions();
            List<QuestionResponseDto> questionReadDtoList = _mapper.Map<List<QuestionResponseDto>>(questionList);
            var serviceResponse = new ServiceResponse<List<QuestionResponseDto>>();
            serviceResponse.Data = questionReadDtoList;
            return serviceResponse;
        }

        public ServiceResponse<QuestionResponseDto> RemoveQuestion(int id)
        {
            if (!_questionRepository.Exist(id))
            {
                return new ServiceResponse<QuestionResponseDto>()
                {
                    Message = "Question not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }

            Question questionToDelete = _questionRepository.GetById(id);
            Question questionHolder = new Question()
            {
                Id = questionToDelete.Id,
                Exam = questionToDelete.Exam,
                Content = questionToDelete.Content
            };
            bool isSuceeded = _questionRepository.RemoveQuestion(questionToDelete);
            if (!isSuceeded)
            {
                return new ServiceResponse<QuestionResponseDto>()
                {
                    Message = ErrorMessage.DELETE,
                    StatusCode = HttpStatusCode.InternalServerError,
                };
            }

            QuestionResponseDto questionResponseDto = _mapper.Map<QuestionResponseDto>(questionHolder);
            _messagePublisher.PublishQuestion(questionResponseDto, questionHolder.Exam.Id, EventType.DeleteQuestion);
            return new ServiceResponse<QuestionResponseDto>()
            {
                Data = questionResponseDto,
                Success = true,
                StatusCode = HttpStatusCode.OK,
                Message = SuccessMessage.DELETE
            };

        }

        public ServiceResponse<QuestionResponseDto> UpdateQuestion(int oldQuestionId, QuestionUpdateRequestDto QuestionRequestDto)
        {
            if (!_questionRepository.Exist(oldQuestionId))
            {
                return new ServiceResponse<QuestionResponseDto>()
                {
                    Message = "Question not found",
                    StatusCode = HttpStatusCode.NotFound
                };
            }


            try
            {
                bool isAnyOptionChanged = false;
                if (QuestionRequestDto.options != null)
                {
                    foreach (OptionUpdateRequestDto optionDto in QuestionRequestDto.options)
                    {
                        bool isOptionChanged = _optionRepository.UpdateOption(optionDto.Id, optionDto);
                        //Send update correct option message 
                        if (isOptionChanged && optionDto.IsCorrect == true)
                        {
                            Question updatedQuestion = _questionRepository.GetById(oldQuestionId);
                            int ExamId = updatedQuestion.Exam.Id;
                            QuestionResponseDto questionResponseDto = _mapper.Map<QuestionResponseDto>(updatedQuestion);
                            _messagePublisher.PublishQuestion(questionResponseDto, ExamId, EventType.UpdateQuestion);
                        }
                        if (!isAnyOptionChanged)
                        {
                            isAnyOptionChanged = isOptionChanged;
                        }
                    }

                }

                QuestionUpdateRequestDto newQuestionUpdateDto = new QuestionUpdateRequestDto()
                {
                    Content = QuestionRequestDto.Content,
                    Score = QuestionRequestDto.Score,
                    TimeLimit = QuestionRequestDto.TimeLimit
                };

                bool isChanged = _questionRepository.UpdateQuestion(oldQuestionId, newQuestionUpdateDto);
                if (!(isChanged || isAnyOptionChanged))
                {

                    return new ServiceResponse<QuestionResponseDto>()
                    {
                        Message = SuccessMessage.UNCHANGE,
                        StatusCode = HttpStatusCode.OK
                    };
                }
                if (isChanged)
                {
                    Question updatedQuestion = _questionRepository.GetById(oldQuestionId);
                    int ExamId = updatedQuestion.Exam.Id;
                    QuestionResponseDto questionResponseDto = _mapper.Map<QuestionResponseDto>(updatedQuestion);
                    _messagePublisher.PublishQuestion(questionResponseDto, ExamId, EventType.UpdateQuestion);
                }
            }
            catch (Exception ex)
            {
                return new ServiceResponse<QuestionResponseDto>()
                {
                    Message = ex.Message,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }



            return new ServiceResponse<QuestionResponseDto>()
            {
                Success = true,
                Message = SuccessMessage.UPDATE,

            };
        }
    }
}
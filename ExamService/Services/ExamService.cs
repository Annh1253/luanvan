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
    public class ExamsService : IExamService
    {
        private readonly IExamRepository _examRepository;
        private readonly IMapper _mapper;
        private readonly IMessagePublisher _messagePublisher;
        private readonly ITopicRepository _topicRepository;


        public ExamsService(IExamRepository examRepository, ITopicRepository topicRepository, IMapper mapper, IMessagePublisher messagePublisher)
        {
            this._topicRepository = topicRepository;
            this._examRepository = examRepository;
            this._mapper = mapper;
            this._messagePublisher = messagePublisher;
        }
        public ServiceResponse<ExamResponseDto> AddExam(int TopicId, ExamRequestDto examRequestDto)
        {
            Exam examEntity = _mapper.Map<Exam>(examRequestDto);

            if(!_topicRepository.Exist(TopicId)) return new ServiceResponse<ExamResponseDto>() {
                StatusCode = HttpStatusCode.NotFound,
                Message = "Topic not found"
            };
          
            Topic topic = _topicRepository.GetById(TopicId);
            examEntity.Topic = topic;

            bool SavedSucceeded = _examRepository.AddExam(examEntity);   
            
            // if guard
            if (!SavedSucceeded) return new ServiceResponse<ExamResponseDto>() {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = ErrorMessage.CREATE
            };
            
            ExamResponseDto examReadDto = _mapper.Map<ExamResponseDto>(examEntity);
            var serviceResponse = new ServiceResponse<ExamResponseDto>();
            serviceResponse.Data = examReadDto;
            serviceResponse.StatusCode = HttpStatusCode.Created;
            serviceResponse.Success = true;

            _messagePublisher.PublishExam(examReadDto, EventType.NewExamCreate);

            return serviceResponse;
        }

        public bool Exist(int id)
        {
           return _examRepository.Exist(id);
        }

        public ServiceResponse<ExamResponseDto> GetById(int id)
        {
            Exam examEntity = _examRepository.GetById(id);
         
            
            // if guard
            if (examEntity == null) return new ServiceResponse<ExamResponseDto>() {
                StatusCode = HttpStatusCode.NotFound,
                Message = "Exam not found"
            };
            
            ExamResponseDto examReadDto = _mapper.Map<ExamResponseDto>(examEntity);
            var serviceResponse = new ServiceResponse<ExamResponseDto>();
            serviceResponse.Data = examReadDto;
            serviceResponse.Success = true;
            return serviceResponse;
        }

        public ServiceResponse<List<ExamResponseDto>> GetExams(ExamFilterDto examFilter)
        {
            List<Exam> examList = new List<Exam>();
            if(examFilter.hasQuestion)
            {
                examList = _examRepository.GetAllExams();
            }else{
                Console.WriteLine("Fetching null question exam....");
                examList = _examRepository.GetAllExamsWithNoQuestion();
            }

            foreach(Exam exam in examList)
            {
                foreach(Question question in exam.Questions)
                {
                    question.Options.Sort((a,b)=> a.Id.CompareTo(b.Id));
                }
            }
            List<ExamResponseDto> examReadDtoList = _mapper.Map<List<ExamResponseDto>>(examList);
            var serviceResponse = new ServiceResponse<List<ExamResponseDto>>();
            serviceResponse.Data = examReadDtoList;
            serviceResponse.Success = true;
            return serviceResponse;
        }

        public ServiceResponse<List<ExamResponseDto>> GetExamsByAuthorEmail(string AuthorEmail)
        {
            var serviceResponse = new ServiceResponse<List<ExamResponseDto>>();
            List<Exam> examList = _examRepository.GetAllExamsByAuthor(AuthorEmail);
            if(examList == null){
                serviceResponse.StatusCode = HttpStatusCode.NotFound;
                serviceResponse.Message = "No Exam was found";

                return serviceResponse;
            }

            try{
             
                List<ExamResponseDto> examResponseDtoList = _mapper.Map<List<ExamResponseDto>>(examList);

                serviceResponse.Success = true;
                serviceResponse.Data = examResponseDtoList;
                serviceResponse.StatusCode = HttpStatusCode.OK;

            }catch(Exception ex){

                serviceResponse.StatusCode = HttpStatusCode.InternalServerError;
                serviceResponse.Message = ex.Message;
            }         
            
            return serviceResponse;
        }

        

        public ServiceResponse<List<ExamResponseDto>> GetExamsByTopicName(string topicName)
        {
            var serviceResponse = new ServiceResponse<List<ExamResponseDto>>();
            if(!_topicRepository.ExistByName(topicName)){
                serviceResponse.StatusCode = HttpStatusCode.NotFound;
                serviceResponse.Message = "Topic not found";

                return serviceResponse;
            }

            try {
                Topic topic = _topicRepository.GetByName(topicName);
                Console.WriteLine(topic.Id);
                List<Exam> examList = _examRepository.GetAllExamsByTopic(topic);
                Console.WriteLine(examList.Count);
                List<ExamResponseDto> examResponseDtoList = _mapper.Map<List<ExamResponseDto>>(examList);

                serviceResponse.Success = true;
                serviceResponse.Data = examResponseDtoList;
                serviceResponse.StatusCode = HttpStatusCode.OK;

            } catch(Exception ex){

                serviceResponse.StatusCode = HttpStatusCode.InternalServerError;
                serviceResponse.Message = ex.Message;
            }         
            
            return serviceResponse;
        }

        public ServiceResponse<List<ExamResponseDto>> GetExamsWithNoQuestion()
        {
            throw new NotImplementedException();
        }

        public ServiceResponse<ExamResponseDto> RemoveExam(int ExamId)
        {
            if(!_examRepository.Exist(ExamId))
            {
                return new ServiceResponse<ExamResponseDto>()
                { 
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Exam not found"
                };
            }

            Exam examToDelete = _examRepository.GetById(ExamId);
            bool isSuceeded = _examRepository.RemoveExam(examToDelete);
            if(!isSuceeded)
            {
                return new ServiceResponse<ExamResponseDto>()
                { 
                    Message = "Something has gone wrong while deleting",
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }
            
            var serviceResponse = new ServiceResponse<ExamResponseDto>()
                {
                    Success = true,
                    Message = "Delete Successfully",
                    StatusCode = HttpStatusCode.OK
                };
            return serviceResponse;
        }

        public ServiceResponse<ExamResponseDto> UpdateExam(int oldExamId, ExamUpdateRequestDto examUpdateRequestDto)
        {
            if(!_examRepository.Exist(oldExamId))
            {
                return new ServiceResponse<ExamResponseDto>()
                        {
                            Message = "Exam not found",
                            StatusCode = HttpStatusCode.NotFound
                        };
            }

            bool isSuceeded = _examRepository.UpdateExam(oldExamId, examUpdateRequestDto);
            if(!isSuceeded)
            {
                return new ServiceResponse<ExamResponseDto>()
                { 
                    Message = ErrorMessage.UPDATE,
                    StatusCode = HttpStatusCode.InternalServerError
                };
            }

            ServiceResponse<ExamResponseDto> serviceResponse = new ServiceResponse<ExamResponseDto>()
            {
                Message = SuccessMessage.UPDATE,
                Success = true,
                StatusCode = HttpStatusCode.OK
            };

            return serviceResponse;
        }
    }
}
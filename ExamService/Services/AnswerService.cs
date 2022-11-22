using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExamService.Contracts.RepositoryContracts;
using ExamService.Contracts.ServiceContracts;
using ExamService.Dtos.Answer;
using ExamService.Models;
using ExamService.Response;

namespace ExamService.Services
{
    public class AnswerService : IAnswerService
    {
        private readonly IAnswerRepository _answerRepository;
        private readonly IMapper _mapper;

        public AnswerService(IAnswerRepository answerRepository, IMapper mapper)
        {
            this._answerRepository = answerRepository;
            this._mapper = mapper;
        }
        public ServiceResponse<AnswerResponseDto> AddAnswer(AnswerRequestDto AnswerRequestDto)
        {
            throw new NotImplementedException();
        }

        public ServiceResponse<AnswerResponseDto> AddAnswers(List<AnswerRequestDto> AnswerRequestDto)
        {
            throw new NotImplementedException();
        }

        public bool Exist(int id)
        {
            throw new NotImplementedException();
        }

        public ServiceResponse<List<AnswerResponseDto>> GetAnswers()
        {
            List<Answer> answerList = _answerRepository.GetAllAnswers();
            List<AnswerResponseDto> answerReadDtoList = _mapper.Map<List<AnswerResponseDto>>(answerList);
            var serviceResponse = new ServiceResponse<List<AnswerResponseDto>>();
            serviceResponse.Success = true;
            serviceResponse.Data = answerReadDtoList;
            return serviceResponse;
        }

        public ServiceResponse<AnswerResponseDto> GetById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
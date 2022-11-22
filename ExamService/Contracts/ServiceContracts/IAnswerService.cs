using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Dtos.Answer;
using ExamService.Response;

namespace ExamService.Contracts.ServiceContracts
{
    public interface IAnswerService
    {
        ServiceResponse<List<AnswerResponseDto>> GetAnswers();
        ServiceResponse<AnswerResponseDto> GetById(int id);
 
        ServiceResponse<AnswerResponseDto> AddAnswer(AnswerRequestDto AnswerRequestDto);
        ServiceResponse<AnswerResponseDto> AddAnswers(List<AnswerRequestDto> AnswerRequestDto);
        bool Exist(int id);
    }
}
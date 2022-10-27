using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Dtos;
using ExamService.Response;

namespace ExamService.Contracts.ServiceContracts
{
    public interface IQuestionService
    {
        ServiceResponse<List<QuestionResponseDto>> GetQuestions();
        ServiceResponse<QuestionResponseDto> GetById(int id);
 
        ServiceResponse<QuestionResponseDto> AddQuestion(int ExamId, QuestionRequestDto questionRequestDto);

        bool Exist(int id);
    }
}
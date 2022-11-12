using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Dtos;
using ExamService.Response;

namespace ExamService.Contracts.ServiceContracts
{
    public interface IExamService
    {
        ServiceResponse<List<ExamResponseDto>> GetExams(ExamFilterDto examFilter);
        ServiceResponse<List<ExamResponseDto>> GetExamsWithNoQuestion();
        ServiceResponse<List<ExamResponseDto>> GetExamsByTopicName(string topicName);
        ServiceResponse<List<ExamResponseDto>> GetExamsByAuthorEmail(string AuthorEmail);


        ServiceResponse<ExamResponseDto> GetById(int id);
        ServiceResponse<ExamResponseDto> RemoveExam(int id);

        ServiceResponse<ExamResponseDto> AddExam(int TopicId, ExamRequestDto examRequestDto);
        ServiceResponse<ExamResponseDto> UpdateExam(int oldExamId, ExamUpdateRequestDto examRequestDto);

        bool Exist(int id);
    }
}
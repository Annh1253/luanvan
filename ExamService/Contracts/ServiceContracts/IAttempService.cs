using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Dtos;
using ExamService.Response;

namespace ExamService.Contracts.ServiceContracts
{
    public interface IAttempService
    {
        ServiceResponse<List<AttempResponseDto>> GetAttemps(AttempRequestFilter AttempFilter);

        ServiceResponse<List<AttempResponseDto>> GetAttempsByCriteria(AttempRequestFilter AttempFilter);

        ServiceResponse<AttempResponseDto> GetById(int id);
        ServiceResponse<AttempResponseDto> RemoveAttemp(int AttempId);
        ServiceResponse<AttempResponseDto> AddAttemp(AttempRequestDto AttempRequestDto);
        ServiceResponse<AttempResponseDto> AddAttemps(List<AttempRequestDto> AttempRequestDto);
        bool Exist(int id);
    }
}
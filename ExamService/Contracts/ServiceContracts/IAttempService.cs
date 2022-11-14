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
        ServiceResponse<List<AttempResponseDto>> GetAttemps();
        ServiceResponse<AttempResponseDto> GetById(int id);
 
        ServiceResponse<AttempResponseDto> AddAttemp(AttempRequestDto AttempRequestDto);
        ServiceResponse<AttempResponseDto> AddAttemps(List<AttempRequestDto> AttempRequestDto);
        bool Exist(int id);
    }
}
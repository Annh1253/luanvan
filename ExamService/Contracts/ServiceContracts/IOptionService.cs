using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Dtos;
using ExamService.Response;

namespace ExamService.Contracts.ServiceContracts
{
    public interface IOptionService
    {
        ServiceResponse<List<OptionResponseDto>> GetOptions();
        ServiceResponse<OptionResponseDto> GetById(int id);
 
        ServiceResponse<OptionResponseDto> AddOption(int QuestionId, OptionRequestDto optionRequestDto);

        bool Exist(int id);
    }
}
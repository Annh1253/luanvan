using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Dtos;
using ExamService.Response;

namespace ExamService.Contracts.ServiceContracts
{
    public interface IUserService
    {
        ServiceResponse<List<UserResponseDto>> GetUsers();
    }
}
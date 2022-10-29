using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Dtos;
using ExamService.Models;

namespace ExamService.Contracts.RepositoryContracts
{
    public interface IOptionRepository
    {
        List<Option> GetAllOptions();

        Option GetById(int id);

        bool AddOption(Option option);
        public bool UpdateOption(int OldOptionId , OptionUpdateRequestDto newOption);
        public bool RemoveOption(Option question);
        bool Exist(int id);
    }
}
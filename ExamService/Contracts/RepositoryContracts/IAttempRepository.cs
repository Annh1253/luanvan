using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Models;

namespace ExamService.Contracts.RepositoryContracts
{
    public interface IAttempRepository
    {
        List<Attemp> GetAllAttemps();

        Attemp GetById(int id);

        bool AddAttemp(Attemp Attemp);

        bool AddAttemps(List<Attemp> Attemp);

      
        bool Exist(int id);
    }
}
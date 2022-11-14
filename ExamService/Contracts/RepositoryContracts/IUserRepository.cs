using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Dtos;
using ExamService.Models;

namespace ExamService.Contracts.RepositoryContracts
{
    public interface IUserRepository
    {
        List<User> GetAllUsers();

        User GetById(int id);

         User GetByEmail(string Email);

        bool AddUser(User user);
        public bool UpdateUser(int OldUserId , UserRequestDto newUser);
        public bool RemoveUser(User user);
        bool Exist(int id);
        bool ExistByEmail(string email);

    }
}
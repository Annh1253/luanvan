using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Contracts.RepositoryContracts;
using ExamService.Dtos;
using ExamService.Helpers;
using ExamService.Models;
using ExamService.Data;

namespace ExamService.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly DBContext _dbContext;

        public UserRepository(DBContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public bool AddUser(User user)
        {
            try{
                _dbContext.Users.Add(user);
                return SaveChanges();
            }catch(Exception ex){
                throw ex;
            }
        }

        public bool Exist(int id)
        {
            return _dbContext.Users.Any(u => u.Id == id);
        }

        public List<User> GetAllUsers()
        {
            return _dbContext.Users.ToList();
        }

        public User GetById(int id)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Id == id);
        }

        public bool RemoveUser(User user)
        {
            try{
                _dbContext.Users.Remove(user);
                return SaveChanges();
            }catch(Exception ex){
                throw ex;
            }
        }

        public bool UpdateUser(int OldUserId, UserRequestDto newUser)
        {
            try{
                User oldUser = _dbContext.Users.First(e => e.Id == OldUserId);
                CRUDHelper.CopyNoneNull(newUser, oldUser);
                return SaveChanges();
            }catch(Exception ex){
                throw ex;
            }
        }

        public bool SaveChanges(){
            return _dbContext.SaveChanges()>0;
        }

        public bool ExistByEmail(string email)
        {
            return _dbContext.Users.Any(u => u.Email == email);
        }

        public User GetByEmail(string Email)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Email == Email);
        }
    }
}
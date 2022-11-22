
using System.Linq.Expressions;
using ExamService.Contracts.RepositoryContracts;
using ExamService.Data;
using ExamService.Models;

namespace ExamService.Repositories
{
    public class AttempRepository : IAttempRepository
    {
        private readonly DBContext _dbContext;

        public AttempRepository(DBContext dbContext)
        {
            this._dbContext = dbContext;
        }
        public bool AddAttemp(Attemp Attemp)
        {
            try
            {
                _dbContext.Attemps.Add(Attemp);
                return SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool AddAttemps(List<Attemp> Attemp)
        {
            try
            {
                _dbContext.Attemps.AddRange(Attemp);
                return SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool Exist(int id)
        {
            return _dbContext.Attemps.Any(attemp => attemp.Id == id);
        }

        public List<Attemp> GetAllAttemps()
        {
            try
            {
                return _dbContext.Attemps.ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<Attemp> GetAllAttempsWithPredicate(Expression<Func<Attemp, bool>> AttempPredicate)
        {
            return _dbContext.Attemps.Where(AttempPredicate).ToList();
        }

        public Attemp GetById(int id)
        {
            try
            {
                return _dbContext.Attemps.FirstOrDefault(attemp => attemp.Id == id);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool RemoveAttemp(Attemp Attemp)
        {
            try
            {
                _dbContext.Attemps.Remove(Attemp);
                return SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool SaveChanges()
        {
            return _dbContext.SaveChanges() > 0;
        }
    }
}
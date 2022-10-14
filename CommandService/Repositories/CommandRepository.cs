
using CommandService.Contracts;
using CommandService.Data;
using CommandService.Models;

namespace CommandService.Repositories
{
    public class CommandRepository : ICommandRepository
    {
        private readonly AppDbContext _context;

        public CommandRepository(AppDbContext context)
        {
            this._context = context;
        }
        public void CreateCommand(int PlatformId, Command command)
        {
            command.PlatformId = PlatformId;
            _context.Commands.Add(command);
            SaveChanges();
        }

        public void CreatePlatform(Platform platform)
        {
            _context.Platforms.Add(platform);
            SaveChanges();
        }

        public IEnumerable<Platform> GetAllPlatform()
        {
            return _context.Platforms.ToList();
        }

        public Command GetCommand(int CommandId)
        {
            return _context.Commands.FirstOrDefault(c => c.Id == CommandId);
        }

        public IEnumerable<Command> GetCommandsOfPlatform(int PlatformId)
        {
            return _context.Commands.Where(c => c.PlatformId == PlatformId).ToList();
        }

        public bool PlatformExist(int Id)
        {
            return _context.Platforms.Any(p => p.Id == Id);
        }

        public bool ExternalPlatformExist(int platformId)
        {
            return _context.Platforms.Any(p => p.ExternalId == platformId);
        }

        public bool SaveChanges()
        {
            return _context.SaveChanges()>0;
        }
    }
}
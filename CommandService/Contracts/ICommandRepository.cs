using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandService.Models;

namespace CommandService.Contracts
{
    public interface ICommandRepository
    {
        //Platforms
        IEnumerable<Platform> GetAllPlatform(); 
        bool PlatformExist(int Id);
        void CreatePlatform(Platform platform);
        
        //Command
        IEnumerable<Command> GetCommandsOfPlatform(int PlatformId);
        Command GetCommand(int CommandId);
        void CreateCommand(int PlatformId, Command command);
        

        bool SaveChanges();
    }
}
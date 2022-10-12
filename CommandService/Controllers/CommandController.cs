using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommandService.Contracts;
using CommandService.Dtos;
using CommandService.Models;
using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/[controller]/Platform/{platformId}")]
    public class CommandController : ControllerBase
    {
        public ICommandRepository _CommandRepository { get; }
        private readonly IMapper _mapper;
        public CommandController(ICommandRepository commandRepository, IMapper mapper)
        {
            this._mapper = mapper;
            this._CommandRepository = commandRepository;           
        }

        [HttpGet]
        public ActionResult<IEnumerable<CommandReadDto>> GetCommandsForPlatform(int platformId)
        {
            Console.WriteLine($"---> Getting Command For Platform: {platformId}");
            if(!_CommandRepository.PlatformExist(platformId))
            {
                return NotFound(); 
            } 
            var commands = _CommandRepository.GetCommandsOfPlatform(platformId);
            return Ok(_mapper.Map<IEnumerable<CommandReadDto>>(commands));
            
        }

        [HttpGet("{commandId}", Name = "GetCommandForPlatform")]
        public ActionResult<CommandReadDto> GetCommandForPlatform(int platformId, int commandId)
        {
            Console.WriteLine($"---> Getting Command For Platform: {platformId} {commandId}");
            if(!_CommandRepository.PlatformExist(platformId))
            {
                return NotFound();
            }

            var command = _CommandRepository.GetCommand(platformId);
            if(command == null)
            {
                return NotFound();
            }
            return Ok(_mapper.Map<CommandReadDto>(command));
        }

        [HttpPost]
        public ActionResult<CommandReadDto> CreateCommandForPlatform(int platformId, CommandCreateDto commandCreateDto)
        {
             Console.WriteLine($"---> Creating Command For Platform: {platformId} ");
            if(!_CommandRepository.PlatformExist(platformId))
            {
                return NotFound();
            }
            Command command = _mapper.Map<Command>(commandCreateDto);
            _CommandRepository.CreateCommand(platformId, command);
            _CommandRepository.SaveChanges();
            var commandReadDto = _mapper.Map<CommandReadDto>(command);
            return CreatedAtRoute(nameof(GetCommandForPlatform), new {platformId = platformId, commandId = commandReadDto.Id}, commandReadDto); 
        }
    }
}
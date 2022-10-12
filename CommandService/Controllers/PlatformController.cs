using System.Net;
using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CommandService.Contracts;
using AutoMapper;
using CommandService.Dtos;

namespace CommandService.Controllers
{
    [ApiController]
    [Route("api/Command/[controller]")]
    public class PlatformController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly ICommandRepository _commandRepository;
        public PlatformController(ICommandRepository commandRepository, IMapper mapper)
        {
            this._commandRepository = commandRepository;
            this._mapper = mapper;
            
        }

        [HttpGet]
        public ActionResult<IEnumerable<PlatformReadDto>> GetPlatforms(){
            Console.WriteLine("---> Getting Platforms from Command Service");
            var platforms = _commandRepository.GetAllPlatform(); 
            return Ok(_mapper.Map<IEnumerable<PlatformReadDto>>(platforms));
        }

   


        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("---> Inbound POST # Command Service");
            return Ok("Inbound té ò from Platforms Controller  ");
        }
    }
}
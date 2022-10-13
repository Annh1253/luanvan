using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CommandService.Dtos;
using System.Text.Json;
using CommandService.Contracts;
using CommandService.Models;

namespace CommandService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        private EventType  DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("---> Determine Event");
            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);
            switch(eventType.Event)
            {
                case "Platform_Published":
                    Console.WriteLine("---> Platform Published Event Detected");
                    return EventType.PlatformPublished;
                default:
                    Console.WriteLine("---> Unknown Event");
                    return EventType.Undetermined;
            }
        }
        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            this._mapper = mapper;
            this._scopeFactory = scopeFactory;
            
        }
        public void ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);
            switch(eventType)
            {
                case EventType.PlatformPublished:
                    AddPlatform(message);
                    break;
                default:
                    break;
            }
        }

        private void AddPlatform(string message)
        {
            using(var scope = _scopeFactory.CreateScope())
            {
                var repo = scope.ServiceProvider.GetRequiredService<ICommandRepository>();
                var platformPublishedDto = JsonSerializer.Deserialize<PlatformPublishedDto>(message);
                try
                {
                    var platform = _mapper.Map<Platform>(platformPublishedDto);
                    if(!repo.ExternalPlatformExist(platform.ExternalId))
                    {
                        repo.CreatePlatform(platform);
                        repo.SaveChanges();
                        Console.WriteLine("---> Platform added");
                    }
                    else
                    {
                        Console.WriteLine("---> Platform already exists...");
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"---> Could not add platform to DB: {ex.Message}");
                }
            }
        }


    }

    enum EventType
    {
        PlatformPublished,
        Undetermined
    }
}
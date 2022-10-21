using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using UserService.AsyncDataServices;
using UserService.Dtos;
using UserService.Models;
using UserService.Ultils;

namespace UserService.Helpers
{
    public class MessagePublisher : IMessagePublisher
    {
        private readonly IMessageBusClient _messageBusClient;
        private readonly IMapper _mapper;
        public MessagePublisher(IMessageBusClient messageBusClient, IMapper mapper)
        {
            this._mapper = mapper;
            this._messageBusClient = messageBusClient;
        }

        public bool PublishUser(User user, EventType eventType)
        {
           try
           {
                var userPublishedDto = _mapper.Map<UserPublishedDto>(user);
                userPublishedDto.Event = EventGenerator.generateEvent(eventType);
                _messageBusClient.Publish(userPublishedDto);
           }
           catch(Exception e)
           {
                throw e;
           }
           return true;
        }

        public bool PublishUserDto(UserPublishedDto userPublishedDto, EventType eventType)
        {
           try
           {  
                userPublishedDto.Event = EventGenerator.generateEvent(eventType);
                _messageBusClient.Publish(userPublishedDto);
           }
           catch(Exception e)
           {
                throw e;
           }
           return true;
        }
    }
}
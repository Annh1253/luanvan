using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Dtos;
using UserService.Models;
using UserService.Ultils;

namespace UserService.Helpers
{
    public interface IMessagePublisher
    {
        bool PublishUser(User user, EventType eventType);
        bool PublishUserDto(UserPublishedDto userDto, EventType eventType);
    }
}
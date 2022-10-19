using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserService.Dtos;

namespace UserService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishNewPlatform(UserPublishedDto platformPublishedDto);
    }
}
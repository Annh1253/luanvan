using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.EventProcessing
{
    public interface IEventProcessor
    {
        void ProcessEvent(string message);
    }
}
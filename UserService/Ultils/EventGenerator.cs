using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserService.Ultils
{
    public static class EventGenerator
    {
        public static string generateEvent(EventType eventType)
        {
            switch(eventType)
            {
                case EventType.NewUserCreate:
                    return "NewUserCreate";
                case EventType.DeleteUser:
                    return "DeleteUser";
                case EventType.UpdateUserInfo:
                    return "UpdateUserInfo";
                case EventType.UpdateUserCredential:
                    return "UpdateUserCredential";
                default:
                    return "Undefined";
            }
        }
    }
}
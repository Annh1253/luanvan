using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Ultils
{
    public static class EventGenerator
    {
        public static string generateEvent(EventType eventType)
        {
            switch(eventType)
            {
                case EventType.ExamDone:
                    return "ExamDone";
                case EventType.UpdateQuestionScore:
                    return "UpdateQuestionScore";
                case EventType.NewExamCreate:
                    return "NewExamCreate";
                case EventType.DeleteExam:
                    return "DeleteExam";
                case EventType.UpdateExam:
                    return "UpdateExam";
                case EventType.NewQuestionCreate:
                    return "NewQuestionCreate";
                case EventType.DeleteQuestion:
                    return "DeleteQuestion";
                case EventType.UpdateQuestion:
                    return "UpdateQuestion";
                case EventType.NewOptionCreate:
                    return "NewOptionCreate";
                case EventType.DeleteOption:
                    return "DeleteOption";
                case EventType.UpdateOption:
                    return "UpdateOption";
                default:
                    return "Undefined";
            }
        }
    }
}
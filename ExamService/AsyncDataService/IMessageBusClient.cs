using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Dtos;

namespace ExamService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        void PublishExam(ExamPublishedDto examPublishedDto);
        void PublishQuestion(QuestionPublishedDto questionPublishedDto);
        void PublishOption(OptionPublishedDto optionPublishedDto);
    }
}
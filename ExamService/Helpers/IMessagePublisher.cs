using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Dtos;
using ExamService.Models;
using ExamService.Ultils;

namespace ExamService.Helpers
{
    public interface IMessagePublisher
    {
        bool PublishExam(ExamResponseDto examResponseDto, EventType eventType);
        bool PublishQuestion(QuestionResponseDto questionResponseDto, int examId, EventType eventType);
        bool PublishQuestion(QuestionResponseDto optionResponseDto, EventType eventType);

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ExamService.Dtos;
using ExamService.Ultils;
using ExamService.AsyncDataServices;
using ExamService.Dtos;
using ExamService.Models;
using ExamService.Ultils;
using ExamService.Helpers;

namespace ExamService.Helpers
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



        public bool PublishExam(ExamResponseDto examResponseDto, EventType eventType)
        {
            try
            {
                ExamPublishedDto examPublishedDto = _mapper.Map<ExamPublishedDto>(examResponseDto);
                examPublishedDto.Event = EventGenerator.generateEvent(eventType);
                _messageBusClient.PublishExam(examPublishedDto);
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }



        public bool PublishQuestion(QuestionResponseDto questionResponseDto, int examId, EventType eventType)
        {
            try
            {
                int correctOptionId = -1;
                foreach (OptionResponseDto option in questionResponseDto.Options)
                {
                    if (option.IsCorrect)
                    {
                        correctOptionId = option.Id;
                        break;
                    }
                }
                QuestionPublishedDto questionPublishedDto = new QuestionPublishedDto()
                {
                    ExternalId = questionResponseDto.Id,
                    ExternalExamId = examId,
                    ExternalCorrectAnswerId = correctOptionId,
                    Score = questionResponseDto.Score,
                    TimeLimit = questionResponseDto.TimeLimit,
                    Event = EventGenerator.generateEvent(eventType)
                };
                _messageBusClient.PublishQuestion(questionPublishedDto);
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }

        public bool PublishQuestion(QuestionResponseDto questionResponseDto, EventType eventType)
        {
            try
            {
                int correctOptionId = -1;
                foreach (OptionResponseDto option in questionResponseDto.Options)
                {
                    if (option.IsCorrect)
                    {
                        correctOptionId = option.Id;
                        break;
                    }
                }
                QuestionPublishedDto questionPublishedDto = new QuestionPublishedDto()
                {
                    ExternalId = questionResponseDto.Id,
                    ExternalCorrectAnswerId = correctOptionId,
                    Score = questionResponseDto.Score,
                    TimeLimit = questionResponseDto.TimeLimit,
                    Event = EventGenerator.generateEvent(eventType)
                };
                _messageBusClient.PublishQuestion(questionPublishedDto);
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }
    }
}
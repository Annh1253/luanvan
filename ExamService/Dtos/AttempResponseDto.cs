using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Dtos.Answer;

namespace ExamService.Dtos
{
    public class AttempResponseDto
    {
        public int Id { get; set; }
        public double TotalScore { get; set; }
        public int TotalBonusScore { get; set; }
        public DateTime StartTime { get; set; }
        public string Mode { get; set; }
        public DateTime FinishTime { get; set; }

        public int MaxCorrectStreak { get; set; }
        public ExamResponseDto Exam { get; set; }

        public UserResponseDto User { get; set; }

        public List<AnswerResponseDto> Answers { get; set; }
    }
}
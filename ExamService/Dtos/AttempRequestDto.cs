using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Dtos.Answer;

namespace ExamService.Dtos
{
    public class AttempRequestDto
    {
        public double TotalScore { get; set; }
        public int TotalBonusScore { get; set; }
        public int ExamId { get; set; }
        public string Email { get; set; }
        public string Mode { get; set; }
        public DateTime StartTime { get; set; }

        public DateTime FinishTime { get; set; }

        public int MaxCorrectStreak { get; set; }

        public List<AnswerRequestDto> Answers { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Dtos.ExamDoneDto;

namespace ExamService.Dtos.ExamDone
{
    public class AttempDoneDto
    {
        public string user { get; set; }
        public string mode { get; set; }
        public double totalScore { get; set; }
        public DateTime startTime { get; set; }
        public DateTime finishTime { get; set; }
        public int totalBonusScore { get; set; }
        public int maxCorrectStreak { get; set; }

        public List<OptionDoneDto> answers { get; set; }

        public string ToString()
        {
            string result = $"User's email: {user}\n";
            result += $"Score: {totalScore}\n";
            foreach (var answer in answers)
            {
                result += $" {answer.ToString()}";
            }
            return result;
        }
    }
}
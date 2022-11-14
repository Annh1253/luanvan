using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos.ExamDone
{
    public class ExamDoneDto
    {
        public int ExternalExamId { get; set; }
        public List<AttempDoneDto> Attemps {get;set;}

        public string ToString(){
            string result = $"Exam ID: {ExternalExamId}\n";
            foreach(var attemp in Attemps){
                result += $" {attemp.ToString()}";
            }
            return result;
        }
    }
}
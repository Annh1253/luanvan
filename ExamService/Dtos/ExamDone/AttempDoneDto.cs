
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExamService.Dtos.ExamDoneDto;

namespace ExamService.Dtos.ExamDone
{
    public class AttempDoneDto
    {
        public string user  {get;set;}
        public double score {get;set;}
        public List<OptionDoneDto> answers {get;set;}

        public string ToString(){
            string result = $"User's email: {user}\n";
            result += $"Score: {score}\n";
            foreach(var answer in answers){
                result += $" {answer.ToString()}";
            }
            return result;
        }
    }
}
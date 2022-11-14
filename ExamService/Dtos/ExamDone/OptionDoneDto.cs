using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExamService.Dtos.ExamDoneDto
{
    public class OptionDoneDto
    {
        public int questionId { get; set;}
        public int optionId { get; set;}

        public string ToString(){
            return $"question id: {questionId}, option id: {optionId}\n";
        }
    }
}
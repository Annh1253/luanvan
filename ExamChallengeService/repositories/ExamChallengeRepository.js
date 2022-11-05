const Exam = require("../models/Exam");
const Question = require("../models/Question");


class ExamChallengeRepository{
    async checkCorrectOption(userAnswer){
        const question = await Question.findOne({
            externalId: userAnswer.questionId
        })
        return question.externalCorrectOptionId === userAnswer.optionId;
    }

    async saveExam(exam){
        console.log("Creating exam : ", exam);
        const newExam = new Exam({
            externalId: exam.ExternalId,
        })
        newExam
            .save()
            .then((data) => {
                console.log("Save successfully: ", data);
                return data;
            })
            .catch((err) => {
                throw err;
            });
    }

    async saveQuestion(question){
        console.log("Creating question : ", question);
        const newQuestion = new Question({
            externalId: question.ExternalId,
            externalCorrectOptionId: question.ExternalCorrectAnswerId
        })
        newQuestion
            .save()
            .then(async (data) => {
                console.log("Save successfully: ",data);
                let exam = await Exam.findOne({
                    externalId: question.ExternalExamId
                })
                if(exam == null)
                {
                    console.log("Exam not found: ", question.ExternalExamId);
                    return "Not found exam";
                }else{
                    exam.questions.push({
                        questionId: data._id
                    })
                    exam.save()
                }
               
                return data;
            })
            .catch((err) => {
                throw err;
            });
    }

    async saveOption(option){
        
    }

    async updateOption(option){
        
    }
    
}

module.exports = new ExamChallengeRepository();
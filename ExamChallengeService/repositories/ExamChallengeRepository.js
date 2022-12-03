const Exam = require("../models/Exam");
const Question = require("../models/Question");

class ExamChallengeRepository {

  async getExamById(examId)
  {
    return await Exam.findOne({externalId: parseInt(examId)})
  }

  async checkCorrectOption(userAnswer) {
    const question = await Question.findOne({
      externalId: userAnswer.questionId,
    });
    return question.externalCorrectOptionId == userAnswer.optionId
      ? { isCorrect: true, score: question.score }
      : { isCorrect: false, score: 0 };
  }

  async saveExam(exam) {
    console.log("Creating exam : ", exam);
    const newExam = new Exam({
      externalId: exam.ExternalId,
    });
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

  async saveQuestion(question) {
    console.log("Creating question : ", question);
    const newQuestion = new Question({
      externalId: question.ExternalId,
      externalCorrectOptionId: question.ExternalCorrectAnswerId,
      score: question.Score,
      timeLimit: question.TimeLimit
    });
    newQuestion
      .save()
      .then(async (data) => {
        console.log("Save successfully: ", data);
        let exam = await Exam.findOne({
          externalId: question.ExternalExamId,
        });
        if (exam == null) {
          console.log("Exam not found: ", question.ExternalExamId);
          return "Not found exam";
        } else {
          exam.questions.push({
            questionId: data._id,
          });
          exam.save();
        }

        return data;
      })
      .catch((err) => {
        throw err;
      });
  }

  async getExamOfQuestion(questionId) {
    const question = await Question.findOne({ externalId: questionId });
    const exam = await Exam.findOne({
      "questions.questionId": question._id.valueOf(),
    });
    return exam;
  }

  async loadAllQuestionsOfExam(examId) {
    const exams = await Exam.find({});
   
    const exam = await Exam.findOne({ externalId: examId });
 

    const questions = [];

    for (let i = 0; i < exam.questions.length; i++) {
      let question = await Question.findById(exam.questions[i].questionId);
      if (question != null) {
        questions.push({
          questionId: question.externalId,
          correctAnswer: question.externalCorrectOptionId,
          score: question.score ? question.score : 0,
          timeLimit: question.timeLimit ? question.timeLimit : 0
        });
      }
    }
    return questions;

    // exam.questions.forEach(async (question) => {
    //  Question.findById(question.questionId).then((data) => {
    //     questions.push(data);
    //   }).finally(()=>questions);
    // });
  }

  async saveOption(option) {}

  async updateQuestion(question) {
    console.log("Updating question : ", question);
    const filter = { externalId: question.ExternalId };
    const update = {
      externalCorrectOptionId: question.ExternalCorrectAnswerId,
      score: question.Score,
      timeLimit: question.TimeLimit
    };
    const questionToUpdate = Question.findOneAndUpdate(filter, update)
      .then(async (data) => {
        console.log("Save successfully: ", data);
        return data;
      })
      .catch((err) => {
        throw err;
      });
  }

  async deleteQuestion(question) {
    console.log("Updating question : ", question);
    const filter = { externalId: question.ExternalId };

    const questionToUpdate = Question.findOneAndDelete(filter)
      .then(async (data) => {
        console.log("Save successfully: ", data);
        let exam = await Exam.findOne({ externalId: question.ExternalExamId });
        let indexToDelete = -1;
        exam.questions.forEach((question, index) => {
          if (data._id.valueOf() == question.questionId) {
            indexToDelete = index;
          }
        });
        exam.questions.splice(indexToDelete, 1);

        await exam.save();
      })
      .catch((err) => {
        throw err;
      });
  }
}

module.exports = new ExamChallengeRepository();

class Question {
  questions = [
    {
      questionId: 36,
      correctAnswer: "a",
      score: 10,
    },
  ];

  findQuestionById(questionId) {
    return this.questions.find((question) => question.questionId == questionId);
  }

  checkAnswer(questionId, answer) {
    let question = this.findQuestionById(questionId);
    console.log("Question is: " + question);
    return question.correctAnswer == answer
      ? { isCorrect: true, score: question.score }
      : { isCorrect: false, score: 0 };
  }
}

module.exports = Question;

class Question {
  questions = [
    {
      questionId: 36,
      correctAnswer: "a",
      score: 10,
    },
  ];

  findQuestionById(questionId) {
    return this.questions.find(
      (question) => question.questionId == questionId
    );
  }

  checkAnswer(questionId, answer) {
    const question = this.findQuestionById(questionId);
    return question.correctAnswer == answer;
  }
}

module.exports = Question;

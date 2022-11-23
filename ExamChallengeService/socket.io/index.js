const repo = require("../repositories/ExamChallengeRepository");
const socketio = require("socket.io");
const serverName = "ExamServer";
const messagePublisher = require("../AsyncDataService/MessageBusClient");

const formatMessage = require("../utils/message");
const {
  userJoin,
  getCurrentUser,
  userLeave,
  getRoomUsers,
} = require("../utils/user");

const Question = require("../utils/question");
const EventType = require("../EventProcessing/EventType");
const questions = new Question();

const RecieveEventType = {
  START_QUESTION: "start-question",
  START_EXAM: "start-exam",
  CREATE_ROOM: "create-room",
  USER_CONNECT: "connection",
  USER_CHOOSE_OPTION: "user-choose-option",
  USER_DISCONNECT: "disconnect",
  SERVER_SEND_MESSAGE: "server-send-message",
  USER_SEND_MESSAGE: "user-send-message",
  USER_JOIN_ROOM: "user-join-room",
  SERVER_UPDATE_USER: "server-update-user",
  SUBMIT_TEST: "submit-test",
};

const SendEventType = {
  START_QUESTION_SUCCESS: "start-question-success",
  START_EXAM_SUCCESS: "start-exam-success",
  CREATE_ROOM_SUCCESS: "create-room-success",
  CORRECT_ANSWER: "option-is-correct",
  CORRECT_ANSWER_BY_SOE: "option-is-correct-by-soe",
  WRON_ANSWER: "option-is-wrong",
  EXAM_RESULT: "exam-result",
};

const sendQuestionStartTime = function (io) {
  let startTime = Date.now();
  io.to(user.room).emit(SendEventType.START_QUESTION_SUCCESS, { startTime });
};

class SocketIO {
  connect(server) {
    let io = socketio(server, {
      cors: {
        origin: "*",
        methods: ["GET", "POST"],
      },
    });

    //
    io.on(RecieveEventType.USER_CONNECT, function (socket) {
      socket.on(RecieveEventType.CREATE_ROOM, async ({ email, examId }) => {
        console.log("Getting question");

        const questionList = await repo.loadAllQuestionsOfExam(examId);
        questions.questions = questionList;
        console.log(questions.questions);
        socket.emit(SendEventType.CREATE_ROOM_SUCCESS, {
          roomId: email + "_" + examId,
        });
      });

      socket.on(RecieveEventType.USER_JOIN_ROOM, async ({ email, roomId }) => {
        console.log("Someone connect");

        const user = userJoin(socket.id, email, roomId);
        console.log(email, roomId);
        socket.join(user.room);

        io.to(user.room).emit(RecieveEventType.SERVER_UPDATE_USER, {
          room: user.room,
          users: getRoomUsers(user.room),
        });

        socket.emit(
          RecieveEventType.SERVER_SEND_MESSAGE,
          formatMessage(serverName, "Welcome to the chat room")
        );

        socket.broadcast
          .to(user.room)
          .emit(
            RecieveEventType.SERVER_SEND_MESSAGE,
            formatMessage(serverName, `${user.username} has joined the channel`)
          );

        socket.on(RecieveEventType.START_EXAM, () => {
          let startTime = Date.now()
          io.to(user.room).emit(SendEventType.START_EXAM_SUCCESS, {startTime});
        });

        socket.on(RecieveEventType.START_QUESTION, () => {
          let startTime = Date.now();
          io.to(user.room).emit(SendEventType.START_QUESTION_SUCCESS, {
            startTime,
          });
        });

        socket.on(RecieveEventType.USER_CHOOSE_OPTION, (message) => {
          console.log(message);
          const user = getCurrentUser(socket.id);
          user.answers.push({
            questionId: message.questionId,
            optionId: parseInt(message.optionId),
          });

          console.log(user)

          const validateResult = questions.checkAnswer(
            message.questionId,
            message.optionId
          );
          console.log(validateResult);
          if (validateResult.isCorrect) {
            const user = getCurrentUser(socket.id);
            user.totalScore += validateResult.score;
            let startTime = Date.now();
            io.to(user.room).emit(SendEventType.START_QUESTION_SUCCESS, {
              startTime,
            });
            socket.emit(SendEventType.CORRECT_ANSWER, {
              correctAnswer: message.optionId,
              totalScore: user.totalScore,
              score: validateResult.score,
            });
            socket.to(user.room).emit(SendEventType.CORRECT_ANSWER_BY_SOE, {
              correctAnswer: message.optionId,
            });
          } else {
            socket.emit(SendEventType.WRON_ANSWER, {
              wrongAnswer: message.optionId,
              totalScore: user.totalScore,
              score: validateResult.score,
            });
          }
        });

        socket.on(RecieveEventType.SUBMIT_TEST, async function () {
          const user = getCurrentUser(socket.id);
          console.log("user :", user)
          let examId = user.room.split('_').at(-1);
          let exam = await repo.getExamById(examId);
          let payload = {
            ExternalExamId: exam.externalId,
            Attemps: [],
            Event: EventType.ExamDone,
          };
          const users = getRoomUsers(getCurrentUser(socket.id).room);
          io.to(user.room).emit(SendEventType.EXAM_RESULT, {
            room: user.room,
            users: users,
          });
          // console.log("reset data");
          // for (let user1 of users) {
          //   payload.Attemps.push({
          //     user: user1.username,
          //     score: user1.totalScore,
          //     answers: user1.answers,
          //   });
          //   user1.totalScore = 0;
          //   user1.answers = [];
          // }
          // console.log(payload);
          // console.log(payload.Attemps[0].answers);
          messagePublisher.publishMessage(payload);
        });

        socket.on(RecieveEventType.USER_SEND_MESSAGE, function (message) {
          const user = getCurrentUser(socket.id);

          io.to(user.room).emit(
            RecieveEventType.SERVER_SEND_MESSAGE,
            formatMessage(user.username, message)
          );
        });

        socket.on(RecieveEventType.USER_DISCONNECT, function () {
          const user = userLeave(socket.id);
          if (user) {
            io.to(user.room).emit(RecieveEventType.SERVER_UPDATE_USER, {
              room: user.room,
              users: getRoomUsers(user.room),
            });
            io.to(user.room).emit(
              RecieveEventType.SERVER_SEND_MESSAGE,
              formatMessage(serverName, `${user.username} has left the channel`)
            );
          }
        });
      });
    });

    //Broadcast when user connect
  }
}

module.exports = new SocketIO();

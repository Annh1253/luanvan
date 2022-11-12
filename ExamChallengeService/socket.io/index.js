const repo = require("../repositories/ExamChallengeRepository");
const socketio = require("socket.io");
const serverName = "ExamServer";

const formatMessage = require("../utils/message");
const {
  userJoin,
  getCurrentUser,
  userLeave,
  getRoomUsers,
} = require("../utils/user");

const Question = require("../utils/question");
const questions = new Question();

const RecieveEventType = {
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
  CORRECT_ANSWER: "option-is-correct",
  CORRECT_ANSWER_BY_SOE: "option-is-correct-by-soe",
  WRON_ANSWER: "option-is-wrong",
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
      socket.on(RecieveEventType.USER_JOIN_ROOM, ({ username, room }) => {
        console.log("Someone connect");
        const user = userJoin(socket.id, username, room);

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

        socket.on(RecieveEventType.USER_CHOOSE_OPTION, (message) => {
          const user = getCurrentUser(socket.id);
          user.answers.push({
            questionId: message.questionId,
            optionId: message.optionId,
          });
          const isCorrect = questions.checkAnswer(
            message.questionId,
            message.optionId
          );
          if (isCorrect) {
            const user = getCurrentUser(socket.id);
            user.score += 10;
            socket.emit(SendEventType.CORRECT_ANSWER, {
              correctAnswer: message.optionId,
            });
            socket.to(user.room).emit(SendEventType.CORRECT_ANSWER_BY_SOE, {
              correctAnswer: message.optionId,
            });
          } else {
            socket.emit(SendEventType.WRON_ANSWER, {
              wrongAnswer: message.optionId,
            });
          }
        });

        socket.on(RecieveEventType.SUBMIT_TEST, async function () {
          const user = getCurrentUser(socket.id);
          const exam = await repo.getExamOfQuestion(user.answers[0].questionId);
          const payload = {
            externalExamId: exam.externalId,
            attemps: [],
          };
          const users = getRoomUsers(getCurrentUser(socket.id).room);
          for (let user1 of users) {
            payload.attemps.push({
              user: user1.username,
              score: user1.score,
              answers: user1.answers,
            });
            user1.score = 0;
          }
          console.log(payload.attemps[0].answers);
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

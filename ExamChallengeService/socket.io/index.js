const repo = require("../repositories/ExamChallengeRepository");
const socketio = require("socket.io");
const serverName = "ExamServer";
const messagePublisher = require("../AsyncDataService/MessageBusClient");
const {
  calStreakBonusPoint,
  generateRoomId,
} = require("../helpers/ExamHelper");
const formatMessage = require("../utils/message");
const {
  userJoin,
  getCurrentUser,
  userLeave,
  getRoomUsers,
  getUserByName,
  updateUser,
  removeSocketIdDuplicate
} = require("../utils/user");

const Question = require("../utils/question");
const EventType = require("../EventProcessing/EventType");
const questions = new Question();

const RoomMode = {
  NORMAL: "normal",
  CHALLENGE: "challenge",
};

const RoomList = new Set();

const RecieveEventType = {
  NOT_AVAILABLE_USER: "not-available-user",
  GET_AVAILABLE_USER: "get-available-user",
  INVITE_OTHER_USER: "invite-other-user",
  QUESTION_TIMEOUT: "question-timeout",
  START_QUESTION: "start-question",
  START_EXAM: "start-exam",
  CREATE_ROOM: "create-room",
  USER_CONNECT: "connection",
  AVAILABLE_USER: "available-user",
  USER_CHOOSE_OPTION: "user-choose-option",
  USER_DISCONNECT: "disconnect",
  SERVER_SEND_MESSAGE: "server-send-message",
  USER_SEND_MESSAGE: "user-send-message",
  USER_JOIN_ROOM: "user-join-room",
  SERVER_UPDATE_USER: "server-update-user",
  SUBMIT_TEST: "submit-test",
};

const SendEventType = {
  SERVER_SEND_AVAILABLE_USERS: "server-send-available-user",
  INVITE: "invite",
  ROOM_NOT_FOUND: "room-not-found",
  JOIN_ROOM_SUCCESS: "join-room-success",
  START_QUESTION_SUCCESS: "start-question-success",
  START_EXAM_SUCCESS: "start-exam-success",
  CREATE_ROOM_SUCCESS: "create-room-success",
  CORRECT_ANSWER: "option-is-correct",
  CORRECT_ANSWER_BY_SOE: "option-is-correct-by-soe",
  WRON_ANSWER: "option-is-wrong",
  EXAM_RESULT: "exam-result",
};

let answersSet = new Set();
let modeInRoom = "";
let availableUsers = [];

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
      socket.on(RecieveEventType.AVAILABLE_USER, async ({ email }) => {
        console.log(
          `LISTEN AVAILABLE_USER EVENT with Email: ${email} and SocketId: ${socket.id}`
        );
        // userJoin(socket.id, email);
        const availableUser = { id: socket.id, email: email };
        availableUsers = availableUsers.filter(user => {
          return user.email !== email
        });
        availableUsers.push(availableUser);
        availableUsers = removeSocketIdDuplicate(availableUsers);
      });

      socket.on(RecieveEventType.GET_AVAILABLE_USER, ()=>{
        socket.emit(SendEventType.SERVER_SEND_AVAILABLE_USERS, availableUsers);
      })

      socket.on(RecieveEventType.NOT_AVAILABLE_USER, ({email})=>{
        console.log("clearing a user: ", email)
        availableUsers = availableUsers.filter(user => {
          return user.email !== email
        })
        console.log("available users: ", availableUsers)
      })

      socket.on(
        RecieveEventType.CREATE_ROOM,
        async ({ email, examId, mode }) => {
          console.log("Getting question");
          modeInRoom = mode;
          const generatedRoomId = generateRoomId(email, examId);
          RoomList.add(generatedRoomId);
          const questionList = await repo.loadAllQuestionsOfExam(examId);
          questions.questions = questionList;
       
          socket.emit(SendEventType.CREATE_ROOM_SUCCESS, {
            roomId: generatedRoomId,
          });
        }
      );

      socket.on(RecieveEventType.USER_JOIN_ROOM, async ({ email, roomId }) => {
        console.log(
          `LISTEN USER_JOIN_ROOM Event with email: ${email} and roomId: ${roomId}`
        );

        if (!RoomList.has(roomId)) {
          console.log("EMIT: Room_Not_Found event with RoomId: ", roomId);
          socket.emit(SendEventType.ROOM_NOT_FOUND);
          return;
        }

        console.log("Someone connect");

        const user = userJoin(socket.id, email, roomId);
        // updateUser(email, roomId);
        // const user = getUserByName(email);

        console.log(email, roomId);
        socket.join(user.room);
        socket.emit(SendEventType.JOIN_ROOM_SUCCESS, { roomId });
        console.log("EMIT: Join_room_success event with RoomId: ", roomId);

        io.to(user.room).emit(RecieveEventType.SERVER_UPDATE_USER, {
          room: user.room,
          users: getRoomUsers(user.room),
          modeInRoom: modeInRoom,
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

        socket.on(RecieveEventType.INVITE_OTHER_USER, ({ email }) => {
          if(availableUsers.length === 0)
            return;
          // const userToSend = getUserByName(email);
          const availableUser = availableUsers.find( user => user.email === email);
          socket.to(availableUser.id).emit(SendEventType.INVITE, {
            roomId: user.room,
            mode: modeInRoom,
          });
        });

        socket.on(RecieveEventType.QUESTION_TIMEOUT, (questionId) => {
          console.log("Time out: " + questionId);
          answersSet.add(questionId);
          console.log("Answer set: ", answersSet);

          const user = getCurrentUser(socket.id);

          user.streak = 0;
          if (
            answersSet.size > user.answerResults.length ||
            user.mode === RoomMode.NORMAL
          ) {
            console.log("Mode : ", user.mode);
            user.answers.push({
              questionId: questionId,
              optionId: null,
              totalTime: null,
              bonus: 0,
              score: 0,
            });
            user.answerResults.push(false);
          }
        });

        socket.on(RecieveEventType.START_EXAM, (mode) => {
          console.log("Rooom mod: ", mode);

          let startTime = Date.now();
          user.startTime = new Date(startTime);
          const users = getRoomUsers(user.room);
          console.log("Number of user: ", users.length);
          users.forEach((user) => {
            if (user.mode == null) user.mode = mode;
          });

          io.to(user.room).emit(SendEventType.START_EXAM_SUCCESS, {
            startTime,
            mode,
          });
        });

        socket.on(RecieveEventType.START_QUESTION, () => {
          let startTime = Date.now();
          if (user.mode === RoomMode.CHALLENGE) {
            io.to(user.room).emit(SendEventType.START_QUESTION_SUCCESS, {
              startTime,
            });
          } else {
            socket.emit(SendEventType.START_QUESTION_SUCCESS, {
              startTime,
            });
          }
        });

        

        socket.on(RecieveEventType.USER_CHOOSE_OPTION, (message) => {
          console.log("User choose option: " + message);
          answersSet.add(message.questionId);
          console.log("Answer set: ", answersSet);
          const user = getCurrentUser(socket.id);

          console.log(user);
          const validateResult = questions.checkAnswer(
            message.questionId,
            message.optionId
          );

          console.log(validateResult);
          if (validateResult.isCorrect) {
            const user = getCurrentUser(socket.id);
            if (answersSet.size > user.answerResults.length)
              user.answerResults.push(true);

            user.streak++;
            user.maxCorrectStreak = Math.max(
              user.streak,
              user.maxCorrectStreak
            );

            const streakBonusPoint = calStreakBonusPoint(user.streak);
            user.totalBonusScore += streakBonusPoint;
            user.totalScore += validateResult.score + streakBonusPoint;

            user.answers.push({
              questionId: message.questionId,
              optionId: parseInt(message.optionId),
              totalTime: parseFloat(message.totalTime),
              bonus: streakBonusPoint,
              score: validateResult.score,
            });

            let startTime = Date.now();

            if (user.mode == RoomMode.NORMAL) {
              socket.emit(SendEventType.START_QUESTION_SUCCESS, {
                startTime,
              });
            } else {
              io.to(user.room).emit(SendEventType.START_QUESTION_SUCCESS, {
                startTime,
              });
            }

            socket.emit(SendEventType.CORRECT_ANSWER, {
              correctAnswer: message.optionId,
              totalScore: user.totalScore,
              score: validateResult.score,
              bonusScore: streakBonusPoint,
              correctStreak: user.streak,
            });

            if (user.mode == RoomMode.CHALLENGE) {
              const users = getRoomUsers(getCurrentUser(socket.id).room);
              console.log("Room member: " + users.length);
              users.forEach((_user) => {
                console.log("Reset streak");
                if (_user.id != user.id) {
                  _user.answers.push({
                    questionId: message.questionId,
                    optionId: null,
                    totalTime: null,
                    bonus: 0,
                    score: 0,
                  });
                  if (answersSet.size > _user.answerResults.length)
                    _user.answerResults.push(false);
                  _user.streak = 0;
                }
              });

              socket.to(user.room).emit(SendEventType.CORRECT_ANSWER_BY_SOE, {
                correctAnswer: message.optionId,
              });
            }

            //reset correct streak of all other users
          } else {
            user.streak = 0;
            if (
              answersSet.size > user.answerResults.length ||
              user.mode === RoomMode.NORMAL
            ) {
              user.answers.push({
                questionId: message.questionId,
                optionId: parseInt(message.optionId),
                totalTime: parseFloat(message.totalTime),
                bonus: 0,
                score: 0,
              });
              user.answerResults.push(false);
            }

            socket.emit(SendEventType.WRON_ANSWER, {
              wrongAnswer: message.optionId,
              totalScore: user.totalScore,
              score: validateResult.score,
              correctStreak: user.streak,
            });
            if (user.mode === RoomMode.NORMAL) {
              const startTime = Date.now();
              socket.emit(SendEventType.START_QUESTION_SUCCESS, {
                startTime,
              });
            }
          }
        });

        socket.on(RecieveEventType.SUBMIT_TEST, async function () {
          const user = getCurrentUser(socket.id);
          console.log("user :", user);
          console.log("User's room: ", user.room);
          let examId = user.room.toString().split("_").at(-1);
          let exam = await repo.getExamById(examId);
          let payload = {
            ExternalExamId: exam.externalId,
            Attemps: [],
            Event: EventType.ExamDone,
          };
          const users = getRoomUsers(getCurrentUser(socket.id).room);

          console.log("reset data");
          io.to(user.room).emit(SendEventType.EXAM_RESULT, {
            room: user.room,
            users: users,
            payload,
            finishTime: Date.now(),
          });

          // for (let user1 of users) {
          user.finishTime = new Date(Date.now());
          user.answers.forEach((answer) => {
            answer.optionId = answer.optionId == null ? 0 : answer.optionId;
            answer.totalTime = answer.totalTime == null ? 0 : answer.totalTime;
          });
          payload.Attemps.push({
            maxCorrectStreak: user.maxCorrectStreak,
            totalBonusScore: user.totalBonusScore,
            user: user.username,
            mode: user.mode,
            totalScore: user.totalScore,
            answers: user.answers,
            startTime: new Date(user.startTime),
            finishTime: new Date(Date.now()),
          });
          // user1.totalScore = 0;
          // user1.answers = [];
          // }
          console.log(user.answers);

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
          console.log(`User disconnect`);
          if (user) {
            io.to(user.room).emit(RecieveEventType.SERVER_UPDATE_USER, {
              room: user.room,
              users: getRoomUsers(user.room),
              modeInRoom: modeInRoom,
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

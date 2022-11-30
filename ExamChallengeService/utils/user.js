const users = [];

// Join user to chat
function userJoin(id, username, room) {
  const user = {
    id,
    mode: null,
    username,
    room,
    totalBonusScore: 0,
    totalScore: 0,
    streak: 0,
    maxCorrectStreak: 0,
    answers: [],
    answerResults: []
  };

  users.push(user);

  return user;
}

// Get current user
function getCurrentUser(id) {
  return users.find((user) => user.id === id);
}

// User leaves chat
function userLeave(id) {
  const index = users.findIndex((user) => user.id === id);

  if (index !== -1) {
    return users.splice(index, 1)[0];
  }
}

// Get room users
function getRoomUsers(room) {
  return users.filter((user) => user.room === room);
}


module.exports = {
  userJoin,
  getCurrentUser,
  userLeave,
  getRoomUsers,
};

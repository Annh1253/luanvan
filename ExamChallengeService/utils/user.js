let users = [];

// Join user to chat
function userJoin(id, username, room) {
  const user = {
    id,
    mode: null,
    username,
    room: room ? room : "",
    totalBonusScore: 0,
    totalScore: 0,
    streak: 0,
    maxCorrectStreak: 0,
    answers: [],
    answerResults: [],
  };

  users.push(user);

  return user;
}

// Get current user
function getCurrentUser(id) {
  return users.find((user) => user.id === id);
}

function getUserByName(username) {
  return users.find((user) => user.username === username);
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
  const usersInRoom = users.filter((user) => user.room === room);
  const usersInRoomUnique = removeSocketIdDuplicate(usersInRoom);

  return usersInRoomUnique;
}

function removeSocketIdDuplicate(users) {
  const uniqueIds = [];

  const unique = users.reverse().filter((element) => {
    const isDuplicate = uniqueIds.includes(element.id);

    if (!isDuplicate) {
      uniqueIds.push(element.id);
      return true;
    }

    return false;
  });

  return unique;
}

function updateUser(email, roomId) {
  users = users.map(user => {
    if(user.username !== email) return user;
    
    user.room = roomId;
    return user;
  });
}

module.exports = {
  getUserByName,
  userJoin,
  getCurrentUser,
  userLeave,
  getRoomUsers,
  updateUser,
  removeSocketIdDuplicate,
};

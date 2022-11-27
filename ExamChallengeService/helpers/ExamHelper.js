function calStreakBonusPoint(streakCount) {
  const filterStreak = streakCount - 1;
  if(filterStreak == 0)
    return 0;
  if (filterStreak > 5) return filterStreak * 10;
  return filterStreak * 5;
}

module.exports = {
  calStreakBonusPoint,
};

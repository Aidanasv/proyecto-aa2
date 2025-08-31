export function formatTime(seconds: number) {
  const min = Math.floor(seconds / 60);
  const sec = seconds % 60;
  const formattedSec = sec < 10 ? '0' + sec : sec;
  return `${min}:${formattedSec}`;
}
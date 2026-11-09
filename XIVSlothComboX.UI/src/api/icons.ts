/** Mirrors the icon rule of the plugin's icon service. */
export function jobIconId(jobId: number): number {
  if (jobId === 0) return 62571
  if (jobId === 51) return 62116
  return 62100 + jobId
}

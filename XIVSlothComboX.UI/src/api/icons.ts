/**
 * The pictures of the game.
 *
 * The program does not read the files of the game, so it cannot produce the
 * pictures itself. They are taken from the public hosts that serve the game
 * data instead.
 */

/**
 * The hosts the pictures are taken from.
 *
 * The first one is the international host, which serves the pictures as png
 * and hands them out itself. The second one is the mirror that the Chinese
 * community keeps, which serves them as webp; it hands them out through
 * another host, so it is only used when the first one cannot be reached.
 */
export const iconHosts: readonly string[] = [
  'https://xivapi.com',
  'https://cafemaker.wakingsands.com',
]

/** Mirrors the icon rule of the plugin's icon service. */
export function jobIconId(jobId: number): number {
  if (jobId === 0) return 62571
  if (jobId === 51) return 62116
  return 62100 + jobId
}

/**
 * Builds the address of a game icon.
 *
 * The number of an icon is the number of the file that holds the picture in
 * the files of the game, so the address is made out of it: the folder of the
 * picture is the number without its last three digits, the name of the file
 * is the number itself.
 *
 * @param iconId The number of the icon.
 * @param hd Whether the picture in high resolution is wanted.
 * @param host The host the picture is taken from.
 * @returns The address of the picture, or an empty string when there is none.
 */
export function iconUrl(iconId: number, hd = false, host: string = iconHosts[0]): string {
  if (!Number.isFinite(iconId) || iconId <= 0) return ''

  const id = Math.trunc(iconId)
  const folder = Math.floor(id / 1000) * 1000
  const name = `${String(folder).padStart(6, '0')}/${String(id).padStart(6, '0')}`
  const suffix = hd ? '_hr1' : ''

  return `${host}/i/${name}${suffix}.png`
}

/** Escapes MSBuild-reserved characters in one literal path or value. */
export function escapeMsbuild(value: string) {
  return value.replace(/[%$@();'?*]/g, (character) => {
    const code = character.charCodeAt(0).toString(16).toUpperCase().padStart(2, "0");
    return `%${code}`;
  });
}

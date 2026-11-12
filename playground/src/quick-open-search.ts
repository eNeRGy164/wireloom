export function fuzzyScore(query: string, value: string) {
  const needle = query.toLowerCase();
  const haystack = value.toLowerCase();
  if (!needle) return 0;

  const direct = haystack.indexOf(needle);
  if (direct >= 0) return direct + (haystack.length - needle.length) * 0.01;

  let cursor = 0;
  let gaps = 0;
  for (const character of needle) {
    const found = haystack.indexOf(character, cursor);
    if (found < 0) return Number.POSITIVE_INFINITY;
    gaps += found - cursor;
    cursor = found + 1;
  }
  return 20 + gaps + (haystack.length - needle.length) * 0.01;
}

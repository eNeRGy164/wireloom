// Experimental DDS member-ID provider, authored from RFC 1321 section 3.
// This is protocol compatibility code, not a security primitive.
const shifts = [
  [7, 12, 17, 22],
  [5, 9, 14, 20],
  [4, 11, 16, 23],
  [6, 10, 15, 21],
];
const constants = Array.from(
  { length: 64 },
  (_, index) => Math.floor(Math.abs(Math.sin(index + 1)) * 0x100000000) >>> 0,
);

export function computeMd5(input) {
  const bytes = Uint8Array.from(input);
  const padded = new Uint8Array(Math.ceil((bytes.length + 9) / 64) * 64);
  padded.set(bytes);
  padded[bytes.length] = 0x80;

  const view = new DataView(padded.buffer);
  view.setUint32(padded.length - 8, (bytes.length * 8) >>> 0, true);
  view.setUint32(padded.length - 4, Math.floor(bytes.length / 0x20000000), true);
  const state = [0x67452301, 0xefcdab89, 0x98badcfe, 0x10325476];

  for (let offset = 0; offset < padded.length; offset += 64) {
    let [a, b, c, d] = state;
    for (let index = 0; index < 64; index++) {
      let mix;
      let word;
      const round = index >>> 4;
      switch (round) {
        case 0:
          mix = (b & c) | (~b & d);
          word = index;
          break;
        case 1:
          mix = (d & b) | (~d & c);
          word = (5 * index + 1) % 16;
          break;
        case 2:
          mix = b ^ c ^ d;
          word = (3 * index + 5) % 16;
          break;
        default:
          mix = c ^ (b | ~d);
          word = (7 * index) % 16;
          break;
      }

      const sum = (a + mix + constants[index] + view.getUint32(offset + word * 4, true)) >>> 0;
      const shift = shifts[round][index % 4];
      const next = (b + ((sum << shift) | (sum >>> (32 - shift)))) >>> 0;
      [a, b, c, d] = [d, next, b, c];
    }

    state[0] = (state[0] + a) >>> 0;
    state[1] = (state[1] + b) >>> 0;
    state[2] = (state[2] + c) >>> 0;
    state[3] = (state[3] + d) >>> 0;
  }

  const digest = new Uint8Array(16);
  const digestView = new DataView(digest.buffer);
  state.forEach((word, index) => digestView.setUint32(index * 4, word, true));
  return digest;
}

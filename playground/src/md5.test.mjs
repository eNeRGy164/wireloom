import assert from "node:assert/strict";
import { createHash } from "node:crypto";
import test from "node:test";
import { computeMd5 } from "./md5.mjs";

// RFC 1321 appendix A.5 vectors use literal expectations independent of this provider.
for (const [input, expected] of [
  ["", "d41d8cd98f00b204e9800998ecf8427e"],
  ["a", "0cc175b9c0f1b6a831c399e269772661"],
  ["abc", "900150983cd24fb0d6963f7d28e17f72"],
  ["message digest", "f96b697d7cb7938d525a2f31aaf161d0"],
  ["abcdefghijklmnopqrstuvwxyz", "c3fcd3d76192e4007dfb496cca67e13b"],
  ["ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789", "d174ab98d277d9f5a5611c2c9f419d9f"],
  ["1234567890".repeat(8), "57edf4a22be3c955ac49da2e2107b67a"],
]) {
  test(`RFC vector: ${JSON.stringify(input)}`, () => {
    assert.equal(Buffer.from(computeMd5(new TextEncoder().encode(input))).toString("hex"), expected);
  });
}

for (const input of [
  "color",
  "shapesize",
  "é",
  "e\u0301",
  "😀",
  "\ud800",
  ...[55, 56, 63, 64, 65, 119, 120].map((length) => "a".repeat(length)),
]) {
  test(`Native MD5 parity for ${JSON.stringify(input)}`, () => {
    const bytes = new TextEncoder().encode(input);
    assert.deepEqual(Buffer.from(computeMd5(bytes)), createHash("md5").update(bytes).digest());
  });
}

test("Binary input, repeated calls, and caller-buffer ownership", () => {
  const bytes = Uint8Array.from({ length: 4096 }, (_, index) => index % 256);
  const original = bytes.slice();
  const expected = createHash("md5").update(bytes).digest();
  assert.deepEqual(Buffer.from(computeMd5(bytes)), expected);
  assert.deepEqual(Buffer.from(computeMd5(bytes)), expected);
  assert.deepEqual(bytes, original);
});

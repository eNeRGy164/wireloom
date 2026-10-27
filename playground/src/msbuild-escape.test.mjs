import assert from "node:assert/strict";
import { test } from "node:test";
import { escapeMsbuild } from "./msbuild-escape.ts";

test("escapes MSBuild special characters and preserves literal escape-like text", () => {
  assert.equal(escapeMsbuild("sensor;%3B$(a)@?'*"), "sensor%3B%253B%24%28a%29%40%3F%27%2A");
});

test("leaves ordinary path characters unchanged", () => {
  assert.equal(escapeMsbuild("nested/sensor-v2.idl"), "nested/sensor-v2.idl");
});

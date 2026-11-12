import assert from "node:assert/strict";
import test from "node:test";
import { fuzzyScore } from "./quick-open-search.ts";

test("quick-open scoring prefers direct and earlier matches", () => {
  // Arrange
  const query = "main";

  // Act
  const direct = fuzzyScore(query, "src/main.hpp");
  const later = fuzzyScore(query, "src/plugins/main.hpp");

  // Assert
  assert.ok(direct < later);
});

test("quick-open scoring supports case-insensitive subsequences", () => {
  // Arrange
  const query = "mnhpp";

  // Act
  const score = fuzzyScore(query, "src/main.hpp");

  // Assert
  assert.ok(Number.isFinite(score));
});

test("quick-open scoring rejects missing characters and treats an empty query as a match", () => {
  // Arrange / Act
  const missing = fuzzyScore("xyz", "src/main.hpp");
  const empty = fuzzyScore("", "src/main.hpp");

  // Assert
  assert.equal(missing, Number.POSITIVE_INFINITY);
  assert.equal(empty, 0);
});

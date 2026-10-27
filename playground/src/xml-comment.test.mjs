import assert from "node:assert/strict";
import test from "node:test";
import { xmlCommentEnd } from "./xml-comment.ts";

test("XML comments end with either legal closing delimiter", () => {
  // Arrange
  const terminators = ["-->", "--!>"];

  // Act and Assert
  for (const terminator of terminators) {
    const source = `<!-- comment ${terminator}<Project />`;
    const match = xmlCommentEnd.exec(source);

    assert.equal(match?.[0], terminator);
    assert.equal(match?.index, source.indexOf(terminator));
  }
});

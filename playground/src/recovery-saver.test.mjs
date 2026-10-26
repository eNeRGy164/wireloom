import assert from "node:assert/strict";
import test from "node:test";
import { RecoverySaver } from "./recovery-saver.ts";

function deferred() {
  let resolve;
  let reject;
  const promise = new Promise((resolvePromise, rejectPromise) => {
    resolve = resolvePromise;
    reject = rejectPromise;
  });
  return { promise, resolve, reject };
}

function nextTurn() {
  return new Promise((resolve) => setImmediate(resolve));
}

test("an oversized edit invalidates a pending save status", async () => {
  // Arrange
  const pending = deferred();
  const statuses = [];
  const saver = new RecoverySaver({
    persist: () => pending.promise,
    status: (message) => statuses.push(message),
    unavailable: () => assert.fail("A successful old save must not disable recovery."),
  });
  saver.save({ source: "previous" }, true, true);
  await nextTurn();

  // Act
  saver.save({ source: "too large" }, true, false);
  pending.resolve();
  await nextTurn();

  // Assert
  assert.equal(statuses.at(-1), "Too large to save locally");
  assert.equal(statuses.includes("Saved locally"), false);
});

test("an older failed write cannot disable recovery after a newer write succeeds", async () => {
  // Arrange
  const earlier = deferred();
  const latest = deferred();
  const pending = [earlier, latest];
  const statuses = [];
  let unavailable = false;
  const saver = new RecoverySaver({
    persist: () => pending.shift().promise,
    status: (message) => statuses.push(message),
    unavailable: () => {
      unavailable = true;
    },
  });
  saver.save({ source: "earlier" }, true, true);
  saver.save({ source: "latest" }, true, true);
  await nextTurn();

  // Act
  earlier.reject(new Error("older write failed"));
  latest.resolve();
  await nextTurn();

  // Assert
  assert.equal(unavailable, false);
  assert.equal(statuses.at(-1), "Saved locally");
});

test("invalidating a pending write suppresses its completion", async () => {
  // Arrange
  const pending = deferred();
  const statuses = [];
  const saver = new RecoverySaver({
    persist: () => pending.promise,
    status: (message) => statuses.push(message),
    unavailable: () => assert.fail("Invalidating a pending write must not report a failure."),
  });
  saver.save({ source: "reset" }, true, true);
  await nextTurn();

  // Act
  saver.invalidate();
  pending.resolve();
  await nextTurn();

  // Assert
  assert.equal(statuses.at(-1), "Saving…");
  assert.equal(statuses.includes("Saved locally"), false);
});

test("the current write failure disables recovery and reports its state", async () => {
  // Arrange
  const pending = deferred();
  const statuses = [];
  let unavailable = false;
  const saver = new RecoverySaver({
    persist: () => pending.promise,
    status: (message) => statuses.push(message),
    unavailable: () => {
      unavailable = true;
    },
  });
  saver.save({ source: "current" }, true, true);
  await nextTurn();

  // Act
  pending.reject(new Error("storage failed"));
  await nextTurn();

  // Assert
  assert.equal(unavailable, true);
  assert.equal(statuses.at(-1), "Recovery unavailable");
});

test("an unavailable recovery store does not start a write", () => {
  // Arrange
  const statuses = [];
  let writes = 0;
  const saver = new RecoverySaver({
    persist: async () => {
      writes++;
    },
    status: (message) => statuses.push(message),
    unavailable: () => assert.fail("The store was already unavailable."),
  });

  // Act
  saver.save({ source: "ignored" }, false, true);

  // Assert
  assert.equal(writes, 0);
  assert.deepEqual(statuses, []);
});

test("a synchronous persistence failure follows the storage error path", async () => {
  // Arrange
  const statuses = [];
  let unavailable = false;
  const saver = new RecoverySaver({
    persist: () => {
      throw new Error("storage setup failed");
    },
    status: (message) => statuses.push(message),
    unavailable: () => {
      unavailable = true;
    },
  });

  // Act
  saver.save({ source: "current" }, true, true);
  await nextTurn();

  // Assert
  assert.equal(unavailable, true);
  assert.equal(statuses.at(-1), "Recovery unavailable");
});

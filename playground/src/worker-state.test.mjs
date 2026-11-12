import assert from "node:assert/strict";
import test from "node:test";
import { WorkerRequestState } from "./worker-state.mjs";

function request(requestId, workspaceRevision, profile = "7.7.0", target = "csharp") {
  // Arrange
  return {
    protocolVersion: 5,
    workerGeneration: 0,
    requestId,
    workspaceRevision,
    target,
    profile,
    files: [],
    options: {},
  };
}

function response(value) {
  return { ...value, kind: "compiler", success: true, documents: [], diagnostics: [], error: null };
}

test("pending snapshots stay bounded to the newest request", () => {
  const state = new WorkerRequestState();
  const generation = state.replaceWorker();
  state.enqueue(request(1, 1));
  state.enqueue(request(2, 2));
  assert.equal(state.takePending(), null);
  assert.equal(state.markReady(generation), true);
  assert.equal(state.takePending().requestId, 2);
  assert.equal(state.takePending(), null);
});

test("responses require matching protocol, worker, request, revision, target, and profile identity", () => {
  const state = new WorkerRequestState();
  const generation = state.replaceWorker();
  state.markReady(generation);
  state.enqueue(request(7, 4));
  const sent = state.takePending();
  assert.equal(state.matchesResponse(sent, response(sent), sent.workspaceRevision, sent.target, sent.profile), true);
  for (const field of ["protocolVersion", "workerGeneration", "requestId", "workspaceRevision", "target", "profile"]) {
    const stale = {
      ...response(sent),
      [field]: field === "profile" ? "7.3.1" : field === "target" ? "cpp" : sent[field] + 1,
    };
    assert.equal(
      state.matchesResponse(sent, stale, sent.workspaceRevision, sent.target, sent.profile),
      false,
      `${field} mismatch must be rejected`,
    );
  }
});

test("replacement workers reject old readiness and responses", () => {
  const state = new WorkerRequestState();
  const oldGeneration = state.replaceWorker();
  state.markReady(oldGeneration);
  state.enqueue(request(3, 8));
  const sent = state.takePending();
  const newGeneration = state.replaceWorker();
  assert.equal(state.markReady(oldGeneration), false);
  assert.equal(state.markFailed(oldGeneration), false);
  assert.equal(state.matchesResponse(sent, response(sent), sent.workspaceRevision, sent.target, sent.profile), false);
  assert.equal(state.markReady(newGeneration), true);
});

test("a newer workspace request invalidates the previous response", () => {
  const state = new WorkerRequestState();
  const generation = state.replaceWorker();
  state.markReady(generation);
  state.enqueue(request(1, 1, "7.7.0"));
  const oldRequest = state.takePending();
  state.enqueue(request(2, 2, "7.3.1"));
  assert.equal(state.matchesResponse(oldRequest, response(oldRequest), 2, "csharp", "7.3.1"), false);
  const currentRequest = state.takePending();
  assert.equal(currentRequest.requestId, 2);
  assert.equal(currentRequest.workspaceRevision, 2);
  assert.equal(currentRequest.profile, "7.3.1");
});

test("changing target invalidates outstanding results and failures", () => {
  const state = new WorkerRequestState();
  const generation = state.replaceWorker();
  state.markReady(generation);
  state.enqueue(request(31, 12, "7.7.0", "csharp"));
  const sent = state.takePending();

  assert.equal(state.matchesResponse(sent, response(sent), 12, "cpp", "7.7.0"), false);
  assert.equal(state.isCurrentWorkspaceRequest(sent, 12, "cpp", "7.7.0"), false);
});

test("startup failure can recover by starting a new worker and draining the latest request", () => {
  const state = new WorkerRequestState();
  const failedGeneration = state.replaceWorker();
  state.enqueue(request(11, 12, "7.3.1"));
  assert.equal(state.markFailed(failedGeneration), true);
  const replacementGeneration = state.replaceWorker();
  assert.equal(state.markReady(replacementGeneration), true);
  const retry = state.takePending();
  assert.equal(retry.workerGeneration, replacementGeneration);
  assert.equal(retry.profile, "7.3.1");
  assert.equal(
    state.matchesResponse(retry, response(retry), retry.workspaceRevision, retry.target, retry.profile),
    true,
  );
});

test("source edits invalidate the active response before a replacement request is queued", () => {
  const state = new WorkerRequestState();
  const generation = state.replaceWorker();
  state.markReady(generation);
  state.enqueue(request(9, 4));
  const sent = state.takePending();
  assert.equal(state.matchesResponse(sent, response(sent), 5, sent.target, sent.profile), false);
  assert.equal(state.isCurrent(sent, 5, sent.target, sent.profile), false);
});

test("replacement readiness drains the latest queued request and timeout errors follow workspace identity", () => {
  // Arrange
  const state = new WorkerRequestState();
  const firstGeneration = state.replaceWorker();
  state.markReady(firstGeneration);
  const timedOut = request(21, 31, "7.7.0");
  state.enqueue(timedOut);
  state.takePending();
  state.enqueue(request(22, 32, "7.3.1"));
  const replacementGeneration = state.replaceWorker();

  // Act
  const isCurrentTimeout = state.isCurrentWorkspaceRequest(timedOut, 32, "csharp", "7.3.1");
  const isCurrentReplacement = state.isCurrentWorkspaceRequest(request(22, 32, "7.3.1"), 32, "csharp", "7.3.1");
  const shouldDrainWhenReady = state.markReady(replacementGeneration) && state.shouldDrain(false);
  const replacementRequest = state.takePending();

  // Assert
  assert.equal(isCurrentTimeout, false);
  assert.equal(isCurrentReplacement, true);
  assert.equal(shouldDrainWhenReady, true);
  assert.equal(replacementRequest.requestId, 22);
  assert.equal(replacementRequest.workerGeneration, replacementGeneration);
  assert.equal(state.shouldDrain(true), false);
});

test("a timed-out current request remains identifiable after worker generation changes", () => {
  // Arrange
  const state = new WorkerRequestState();
  const firstGeneration = state.replaceWorker();
  state.markReady(firstGeneration);
  const sent = request(25, 36);
  state.enqueue(sent);
  state.takePending();

  // Act
  state.replaceWorker();

  // Assert
  assert.equal(state.isCurrent(sent, 36, sent.target, sent.profile), false);
  assert.equal(state.isCurrentWorkspaceRequest(sent, 36, sent.target, sent.profile), true);
  assert.equal(state.isCurrentWorkspaceRequest(sent, 37, sent.target, sent.profile), false);
  assert.equal(state.isCurrentWorkspaceRequest(sent, 36, sent.target, "7.3.1"), false);
});

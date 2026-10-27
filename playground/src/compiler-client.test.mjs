import assert from "node:assert/strict";
import test from "node:test";
import { CompilerClient } from "./compiler-client.ts";

class FakeWorker {
  terminated = false;
  posted = [];
  onerror = null;
  onmessage = null;
  throwOnPost = false;

  postMessage(message) {
    if (this.throwOnPost) throw new Error("postMessage failed");
    this.posted.push(message);
  }

  terminate() {
    this.terminated = true;
  }

  ready() {
    this.onmessage?.({ data: { type: "ready" } });
  }

  respond(request) {
    this.onmessage?.({
      data: {
        type: "result",
        response: {
          protocolVersion: request.protocolVersion,
          workerGeneration: request.workerGeneration,
          requestId: request.requestId,
          workspaceRevision: request.workspaceRevision,
          profile: request.profile,
          kind: "compiler",
          success: true,
          documents: [],
          diagnostics: [],
          error: null,
        },
      },
    });
  }

  send(data) {
    this.onmessage?.({ data });
  }
}

function installFakeTimers(t) {
  const originalWindow = globalThis.window;
  const originalClearTimeout = globalThis.clearTimeout;
  const timers = new Map();
  let nextId = 1;

  globalThis.window = {
    setTimeout(callback, delay) {
      const id = nextId++;
      timers.set(id, {
        delay,
        callback: () => {
          timers.delete(id);
          callback();
        },
      });
      return id;
    },
  };
  globalThis.clearTimeout = (id) => timers.delete(id);

  t.after(() => {
    globalThis.window = originalWindow;
    globalThis.clearTimeout = originalClearTimeout;
  });

  return timers;
}

function snapshot(revision) {
  return {
    workspaceRevision: revision,
    profile: "7.7.0",
    files: [{ path: "/workspace/main.idl", source: `struct Sample${revision} {};`, isGenerationRoot: true }],
    options: {},
  };
}

function nextTurn() {
  return new Promise((resolve) => setImmediate(resolve));
}

test("restarting an active compiler settles old work and drains the newest request", async (t) => {
  // Arrange
  const timers = installFakeTimers(t);
  const workers = [];
  const results = [];
  const failures = [];
  const callbacks = {
    current: () => ({ revision: 2, profile: "7.7.0" }),
    status: () => {},
    result: (response) => results.push(response),
    failure: (message) => failures.push(message),
  };
  const client = new CompilerClient(callbacks, () => {
    const worker = new FakeWorker();
    workers.push(worker);
    return worker;
  });
  const firstStart = client.start();
  workers[0].ready();
  workers[0].ready();
  await firstStart;
  client.enqueue(snapshot(1));
  client.enqueue(snapshot(2));
  const oldWatchdog = [...timers.values()].find((timer) => timer.delay === 10_000)?.callback;

  // Act
  const replacementStart = client.start();
  workers[1].ready();
  await replacementStart;
  await nextTurn();

  // Assert
  assert.equal(workers[0].terminated, true);
  assert.equal(workers[1].posted.length, 1);
  assert.equal(workers[1].posted[0].workspaceRevision, 2);
  assert.equal(timers.size, 1, "Only the replacement request watchdog remains active.");
  assert.equal(failures.length, 0, "The superseded request must not report a current-workspace failure.");

  // A watchdog already queued by the host must not replace a healthy worker.
  oldWatchdog?.();
  assert.equal(workers.length, 2);
  assert.equal(workers[1].terminated, false);

  workers[1].respond(workers[1].posted[0]);
  await nextTurn();
  assert.equal(results.length, 1);
  assert.equal(results[0].workspaceRevision, 2);
  assert.equal(timers.size, 0);
});

test("restarting during startup ignores old worker events and settles the old start", async (t) => {
  // Arrange
  installFakeTimers(t);
  const workers = [];
  const failures = [];
  const client = new CompilerClient(
    {
      current: () => ({ revision: 0, profile: "7.7.0" }),
      status: () => {},
      result: () => {},
      failure: (message) => failures.push(message),
    },
    () => {
      const worker = new FakeWorker();
      workers.push(worker);
      return worker;
    },
  );
  const abandonedStart = client.start();
  const replacementStart = client.start();

  // Act
  workers[0].onerror({ message: "stale worker failed" });
  workers[0].ready();
  workers[1].ready();
  await Promise.all([abandonedStart, replacementStart]);

  // Assert
  assert.equal(workers[0].terminated, true);
  assert.equal(workers[1].terminated, false);
  assert.deepEqual(failures, []);
});

test("startup errors stop the worker and are reported once", async (t) => {
  // Arrange
  const timers = installFakeTimers(t);
  const workers = [];
  const failures = [];
  const client = new CompilerClient(
    {
      current: () => ({ revision: 0, profile: "7.7.0" }),
      status: () => {},
      result: () => {},
      failure: (message) => failures.push(message),
    },
    () => {
      const worker = new FakeWorker();
      workers.push(worker);
      return worker;
    },
  );
  const starting = client.start();

  // Act
  workers[0].send({ type: "startup-error", message: "initialization failed" });
  workers[0].send({ type: "startup-error", message: "duplicate initialization failure" });
  await starting;

  // Assert
  assert.equal(workers[0].terminated, true);
  assert.deepEqual(failures, ["initialization failed"]);
  assert.equal(timers.size, 0);
});

test("startup errors without a message use the fallback explanation", async (t) => {
  // Arrange
  installFakeTimers(t);
  const workers = [];
  const failures = [];
  const client = new CompilerClient(
    {
      current: () => ({ revision: 0, profile: "7.7.0" }),
      status: () => {},
      result: () => {},
      failure: (message) => failures.push(message),
    },
    () => {
      const worker = new FakeWorker();
      workers.push(worker);
      return worker;
    },
  );
  const starting = client.start();

  // Act
  workers[0].onerror({ message: "" });
  await starting;

  // Assert
  assert.deepEqual(failures, ["Compiler worker stopped. Restart to continue."]);
  assert.equal(workers[0].terminated, true);
});

test("startup timeout reports failure and terminates the worker", async (t) => {
  // Arrange
  const timers = installFakeTimers(t);
  const workers = [];
  const failures = [];
  const client = new CompilerClient(
    {
      current: () => ({ revision: 0, profile: "7.7.0" }),
      status: () => {},
      result: () => {},
      failure: (message) => failures.push(message),
    },
    () => {
      const worker = new FakeWorker();
      workers.push(worker);
      return worker;
    },
  );
  const starting = client.start();
  const startupTimeout = [...timers.values()].find((timer) => timer.delay === 30_000);

  // Act
  startupTimeout.callback();
  await starting;

  // Assert
  assert.equal(workers[0].terminated, true);
  assert.match(failures[0], /startup timed out/);
  assert.equal(timers.size, 0);
});

test("worker creation failures are reported without a startup timer", async (t) => {
  // Arrange
  const timers = installFakeTimers(t);
  const failures = [];
  const client = new CompilerClient(
    {
      current: () => ({ revision: 0, profile: "7.7.0" }),
      status: () => {},
      result: () => {},
      failure: (message) => failures.push(message),
    },
    () => {
      throw new Error("worker constructor failed");
    },
  );

  // Act
  await client.start();

  // Assert
  assert.match(failures[0], /worker constructor failed/);
  assert.equal(timers.size, 0);
});

test("worker errors reject the current request and synchronous posting errors clear timers", async (t) => {
  // Arrange
  const timers = installFakeTimers(t);
  const workers = [];
  const failures = [];
  const client = new CompilerClient(
    {
      current: () => ({ revision: 1, profile: "7.7.0" }),
      status: () => {},
      result: () => {},
      failure: (message) => failures.push(message),
    },
    () => {
      const worker = new FakeWorker();
      workers.push(worker);
      return worker;
    },
  );
  const starting = client.start();
  workers[0].ready();
  await starting;

  // Act
  workers[0].throwOnPost = true;
  client.enqueue(snapshot(1));
  await nextTurn();

  // Assert
  assert.match(failures[0], /postMessage failed/);
  assert.equal(timers.size, 0);

  // Act: a worker error message also fails a current request through the normal message path.
  workers[0].throwOnPost = false;
  client.enqueue(snapshot(1));
  workers[0].send({ type: "error", requestId: workers[0].posted[0]?.requestId ?? 2, message: "compile failed" });
  await nextTurn();

  // Assert
  assert.match(failures.at(-1), /compile failed/);
  assert.equal(timers.size, 0);
});

test("a compilation timeout starts a replacement and settles the timed out request", async (t) => {
  // Arrange
  const timers = installFakeTimers(t);
  const workers = [];
  const failures = [];
  const client = new CompilerClient(
    {
      current: () => ({ revision: 1, profile: "7.7.0" }),
      status: () => {},
      result: () => {},
      failure: (message) => failures.push(message),
    },
    () => {
      const worker = new FakeWorker();
      workers.push(worker);
      return worker;
    },
  );
  const starting = client.start();
  workers[0].ready();
  await starting;
  client.enqueue(snapshot(1));
  const requestTimeout = [...timers.values()].find((timer) => timer.delay === 10_000);

  // Act
  requestTimeout.callback();
  await nextTurn();
  workers[1].ready();
  await nextTurn();

  // Assert
  assert.equal(workers.length, 2);
  assert.equal(workers[0].terminated, true);
  assert.match(failures[0], /Compilation timed out/);
  assert.equal(timers.size, 0);
});

test("a request queued before startup is sent when the worker becomes ready", async (t) => {
  // Arrange
  installFakeTimers(t);
  const workers = [];
  const client = new CompilerClient(
    {
      current: () => ({ revision: 1, profile: "7.7.0" }),
      status: () => {},
      result: () => {},
      failure: () => {},
    },
    () => {
      const worker = new FakeWorker();
      workers.push(worker);
      return worker;
    },
  );
  client.enqueue(snapshot(1));
  const starting = client.start();

  // Act
  workers[0].ready();
  await starting;

  // Assert
  assert.equal(workers[0].posted.length, 1);
  assert.equal(workers[0].posted[0].workspaceRevision, 1);
});

test("drains a request queued while another request is in flight", async (t) => {
  // Arrange
  installFakeTimers(t);
  const workers = [];
  const results = [];
  const client = new CompilerClient(
    {
      current: () => ({ revision: 2, profile: "7.7.0" }),
      status: () => {},
      result: (response) => results.push(response.workspaceRevision),
      failure: (message) => assert.fail(message),
    },
    () => {
      const worker = new FakeWorker();
      workers.push(worker);
      return worker;
    },
  );
  const starting = client.start();
  workers[0].ready();
  await starting;
  client.enqueue(snapshot(1));
  client.enqueue(snapshot(2));

  // Act
  workers[0].respond(workers[0].posted[0]);
  await nextTurn();

  // Assert
  assert.equal(workers[0].posted.length, 2);
  assert.equal(workers[0].posted[1].workspaceRevision, 2);
  workers[0].respond(workers[0].posted[1]);
  await nextTurn();
  assert.deepEqual(results, [2]);
});

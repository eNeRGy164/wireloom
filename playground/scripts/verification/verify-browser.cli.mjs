async (page) => {
  const input = __WIRELOOM_INPUT__;
  const browser = page.context().browser();
  const report = {
    phase: input.phase,
    browser: browser.version(),
    timestamp: new Date().toISOString(),
    checks: [],
    requests: [],
  };
  const assert = (condition, message) => {
    if (!condition) throw new Error(message);
  };
  const check = async (name, action) => {
    try {
      report.checks.push({ name, passed: true, details: await action() });
    } catch (error) {
      report.checks.push({ name, passed: false, error: String(error) });
    }
  };
  const open = async (url) => {
    const context = await browser.newContext();
    if (browser.browserType().name() === "chromium")
      await context.grantPermissions(["clipboard-read", "clipboard-write"]);
    context.on("request", (request) =>
      report.requests.push({ url: request.url(), method: request.method(), hasBody: !!request.postData() }),
    );
    const app = await context.newPage();
    app.on("dialog", (dialog) => dialog.accept());
    const started = Date.now();
    await app.goto(url);
    await app.waitForFunction(() => document.querySelector("#status")?.textContent.includes("Compiler ready"), null, {
      timeout: 35_000,
    });
    return { app, context, startupMs: Date.now() - started };
  };
  let importSequence = 0;
  const fillSource = async (app, source) => {
    // Firefox's automated multiline insertText duplicates text in Monaco.
    // Preserve exact multiline probes through import. Single-line edits and
    // oversized-input probes still exercise the real editor on every engine.
    if (browser.browserType().name() !== "chromium" && /[\r\n]/.test(source)) {
      const name = `verification-${++importSequence}.idl`;
      await app.locator("#file").setInputFiles({ name, mimeType: "text/plain", buffer: Buffer.from(source, "utf8") });
      await app.waitForFunction((name) => document.querySelector("#input-name").textContent === name, name);
      return;
    }
    await app.locator("#source").click({ position: { x: 150, y: 45 } });
    // Windows WebKit advertises a Macintosh user agent. Monaco therefore
    // expects Command+A even though Playwright's host shortcut maps to Ctrl+A.
    const selectAll = await app.evaluate(() => (navigator.userAgent.includes("Macintosh") ? "Meta+a" : "Control+a"));
    await app.keyboard.press(selectAll);
    await app.keyboard.press("Backspace");
    if (source) {
      // Importing oversized text would exercise a different limit policy that
      // deliberately preserves the previous result, so use editor input here.
      if (browser.browserType().name() !== "chromium") {
        await app.keyboard.insertText(source);
        return;
      }
      await app.context().grantPermissions(["clipboard-read", "clipboard-write"]);
      await app.evaluate((source) => navigator.clipboard.writeText(source), source);
      await app.keyboard.press("ControlOrMeta+v");
    }
  };
  const compileUi = async (app, source) => {
    await fillSource(app, source);
    await app.waitForFunction(() => document.querySelector("#result-state")?.textContent === "Current", null, {
      timeout: 15_000,
    });
  };
  const hashCase = input.cases.find((value) => value.id.includes("hash") && value.files.length === 1);

  if (["parity", "corpus"].includes(input.phase)) {
    for (const url of [input.root, input.nested]) {
      await check(`published parity and assets: ${url}`, async () => {
        const { app, context, startupMs } = await open(url);
        try {
          const snapshot = await app.locator("body").ariaSnapshot();
          const assets = await app.evaluate(async () => {
            const icon = document.querySelector('link[rel="icon"]');
            const logo = document.querySelector(".brand img");
            await logo.decode();
            const favicon = await fetch(icon.href);
            return {
              icon: icon.href,
              logo: logo.currentSrc,
              logoWidth: logo.naturalWidth,
              faviconStatus: favicon.status,
            };
          });
          assert(
            assets.faviconStatus === 200 && assets.logoWidth > 0 && assets.icon === assets.logo,
            "Square SVG favicon/logo failed to load",
          );
          const uiWorker = app.workers().find((worker) => worker.url().endsWith("/dotnet/worker.mjs"));
          const resourceSizes = uiWorker
            ? await uiWorker.evaluate(() =>
                performance.getEntriesByType("resource").map((entry) => ({
                  name: entry.name,
                  transferSize: entry.transferSize,
                  encodedBodySize: entry.encodedBodySize,
                  decodedBodySize: entry.decodedBodySize,
                })),
              )
            : [];
          const parity = await app.evaluate(
            async ({ cases, expected }) => {
              const base = new URL(".", location.href);
              const worker = new Worker(new URL("dotnet/worker.mjs", base), { type: "module" });
              const wait = (request) =>
                new Promise((resolve, reject) => {
                  const timer = setTimeout(() => {
                    worker.terminate();
                    reject(new Error("Verification worker timed out"));
                  }, 20_000);
                  const onMessage = (event) => {
                    if (!request && event.data.type !== "ready" && event.data.type !== "startup-error") return;
                    if (
                      request &&
                      event.data.requestId !== request.requestId &&
                      event.data.response?.requestId !== request.requestId
                    )
                      return;
                    clearTimeout(timer);
                    worker.removeEventListener("message", onMessage);
                    if (event.data.type === "startup-error" || event.data.type === "error")
                      reject(new Error(event.data.message));
                    else resolve(event.data);
                  };
                  worker.addEventListener("message", onMessage);
                  worker.addEventListener("error", (event) => reject(new Error(event.message)), { once: true });
                  if (request) worker.postMessage(request);
                });
              await wait();
              const results = [];
              let requestId = 0;
              try {
                for (const baseline of expected) {
                  const fixture = cases.find((value) => value.id === baseline.caseId);
                  const request = {
                    protocolVersion: 1,
                    workerGeneration: 1,
                    requestId: ++requestId,
                    workspaceRevision: requestId,
                    profile: baseline.profile,
                    files: fixture.files,
                    options: {},
                  };
                  const started = performance.now();
                  const { response } = await wait(request);
                  const documents = [...response.documents].sort((a, b) =>
                    a.name < b.name ? -1 : a.name > b.name ? 1 : 0,
                  );
                  const diagnostics = [...response.diagnostics].sort(
                    (a, b) =>
                      (a.filePath < b.filePath ? -1 : a.filePath > b.filePath ? 1 : 0) ||
                      a.startOffset - b.startOffset ||
                      (a.id < b.id ? -1 : a.id > b.id ? 1 : 0),
                  );
                  const actual = { success: response.success, documents, diagnostics };
                  const expectedResult = {
                    success: baseline.success,
                    documents: baseline.documents,
                    diagnostics: baseline.diagnostics,
                  };
                  const passed =
                    response.kind === "compiler" &&
                    response.workerGeneration === request.workerGeneration &&
                    response.requestId === request.requestId &&
                    response.workspaceRevision === request.workspaceRevision &&
                    response.profile === request.profile &&
                    (response.success || documents.length === 0) &&
                    JSON.stringify(actual) === JSON.stringify(expectedResult);
                  results.push({
                    caseId: fixture.id,
                    profile: baseline.profile,
                    passed,
                    success: response.success,
                    documentCount: documents.length,
                    diagnostics: response.diagnostics,
                    compileMs: performance.now() - started,
                    failure: passed
                      ? undefined
                      : {
                          expectedDocumentNames: baseline.documents.map((value) => value.name),
                          actualDocumentNames: documents.map((value) => value.name),
                          expectedDiagnostics: baseline.diagnostics,
                        },
                  });
                }
                const fixture = cases[0];
                for (let index = 0; index < 2; index++) {
                  const { response } = await wait({
                    protocolVersion: 1,
                    workerGeneration: 1,
                    requestId: ++requestId,
                    workspaceRevision: requestId,
                    profile: "7.7.0",
                    files: fixture.files,
                    options: {},
                  });
                  if (!response.success) throw new Error("Repeated compilation failed");
                }
                return { results, repeatedCompilation: true };
              } finally {
                worker.terminate();
              }
            },
            { cases: input.cases, expected: input.expected },
          );
          const warmStarted = Date.now();
          await app.reload();
          await app.waitForFunction(
            () => document.querySelector("#status")?.textContent.includes("Compiler ready"),
            null,
            { timeout: 35_000 },
          );
          const warmStartupMs = Date.now() - warmStarted;
          await compileUi(app, hashCase.files[0].source);
          await app.locator("#profile").selectOption("7.3.1");
          await app.waitForFunction(() => document.querySelector("#result-state")?.textContent === "Current");
          const profileSwitchDocuments = await app.locator("#file-list button").allTextContents();
          assert(profileSwitchDocuments.length === 4, "Profile switch did not render every sample document");
          return {
            startupMs,
            warmStartupMs,
            snapshot,
            assets,
            workerResources: resourceSizes.length,
            workerTransferBytes: resourceSizes.reduce((total, value) => total + value.transferSize, 0),
            workerEncodedBytes: resourceSizes.reduce((total, value) => total + value.encodedBodySize, 0),
            profileSwitchDocuments,
            parity,
          };
        } finally {
          await context.close();
        }
      });
    }
    await check("main thread heartbeat stays responsive during real compiler work", async () => {
      const { app, context } = await open(input.root);
      try {
        return await app.evaluate(async () => {
          const worker = new Worker("/dotnet/worker.mjs", { type: "module" });
          await new Promise((resolve, reject) => {
            worker.onmessage = (event) =>
              event.data.type === "ready" ? resolve() : reject(new Error(event.data.message));
            worker.onerror = (event) => reject(new Error(event.message));
          });
          const source = Array.from({ length: 128 }, (_, index) => `struct Load${index} { long value; };`).join("\n");
          const started = performance.now();
          const ticks = [];
          const interval = setInterval(() => ticks.push(performance.now() - started), 16);
          const edit = new Promise((resolve) =>
            setTimeout(() => {
              const field = document.querySelector("#source");
              field.dataset.verificationHeartbeat = "responsive";
              resolve(performance.now() - started);
            }, 30),
          );
          try {
            const response = await new Promise((resolve, reject) => {
              const timer = setTimeout(
                () => reject(new Error("Representative compilation exceeded 10 seconds")),
                10_000,
              );
              worker.onmessage = (event) => {
                clearTimeout(timer);
                event.data.type === "result" ? resolve(event.data.response) : reject(new Error(event.data.message));
              };
              worker.postMessage({
                protocolVersion: 1,
                workerGeneration: 1,
                requestId: 1,
                workspaceRevision: 1,
                profile: "7.7.0",
                files: [{ path: "/workspace/load.idl", source, isGenerationRoot: true }],
                options: {},
              });
            });
            const compileMs = performance.now() - started;
            const editMs = await edit;
            const gaps = ticks.map((tick, index) => tick - (index ? ticks[index - 1] : 0));
            if (!response.success || response.documents.length !== 512 || editMs >= compileMs || editMs > 250)
              throw new Error(
                JSON.stringify({ success: response.success, documents: response.documents.length, editMs, compileMs }),
              );
            return {
              declarations: 128,
              documentCount: response.documents.length,
              compileMs,
              mainThreadTaskMs: editMs,
              heartbeatCount: ticks.length,
              maxHeartbeatGapMs: Math.max(...gaps),
            };
          } finally {
            clearInterval(interval);
            worker.terminate();
          }
        });
      } finally {
        await context.close();
      }
    });
  } else if (input.phase === "faults") {
    await check("missing worker asset shows retry and recovers", async () => {
      const context = await browser.newContext();
      const app = await context.newPage();
      try {
        await app.route("**/dotnet/worker.mjs", (route) => route.abort("failed"));
        await app.goto(input.root);
        await app.locator("#retry").waitFor({ state: "visible" });
        const failure = await app.locator("#status").textContent();
        assert(await app.locator("#download-all").isDisabled(), "Compile remained enabled after startup failure");
        await app.unroute("**/dotnet/worker.mjs");
        await app.locator("#retry").click();
        await app.waitForFunction(
          () => document.querySelector("#status")?.textContent.includes("Compiler ready"),
          null,
          { timeout: 35_000 },
        );
        await compileUi(app, hashCase.files[0].source);
        return { failure, recoveredDocuments: await app.locator("#file-list button").count() };
      } finally {
        await context.close();
      }
    });
    await check("provider failure is a host error and later compilation recovers", async () => {
      const context = await browser.newContext();
      const app = await context.newPage();
      app.on("dialog", (dialog) => dialog.accept());
      try {
        await app.route("**/dotnet/md5.mjs", (route) =>
          route.fulfill({
            contentType: "text/javascript",
            body: 'import { computeMd5 as original } from "./md5.mjs?original"; let calls = 0; export function computeMd5(bytes) { if (calls++ === 0) throw new Error("Injected MD5 provider failure"); return original(bytes); }',
          }),
        );
        await app.goto(input.root);
        await app.waitForFunction(
          () => document.querySelector("#status")?.textContent.includes("Compiler ready"),
          null,
          { timeout: 35_000 },
        );
        await fillSource(app, hashCase.files[0].source);
        await app.waitForFunction(
          () => document.querySelector("#diagnostics-empty")?.textContent.includes("provider"),
          null,
          { timeout: 15_000 },
        );
        const failure = await app.locator("#diagnostics-empty").textContent();
        assert(
          !failure.includes("DDSG0001") && (await app.locator("#file-list button").count()) === 0,
          "Provider fault was misclassified or retained output",
        );
        await compileUi(app, hashCase.files[0].source);
        return { failure, recoveredDocuments: await app.locator("#file-list button").count() };
      } finally {
        await context.close();
      }
    });
    await check("idle worker exception clears output and replacement compiles", async () => {
      const { app, context } = await open(input.root);
      try {
        await compileUi(app, hashCase.files[0].source);
        const worker = app.workers().find((worker) => worker.url().endsWith("/dotnet/worker.mjs"));
        await worker.evaluate(() =>
          setTimeout(() => {
            throw new Error("Injected idle worker failure");
          }, 0),
        );
        await app.locator("#retry").waitFor({ state: "visible" });
        assert((await app.locator("#file-list button").count()) === 0, "Idle failure retained output");
        const failure = await app.locator("#status").textContent();
        await app.locator("#retry").click();
        await app.waitForFunction(
          () => document.querySelector("#status")?.textContent.includes("Compiler ready"),
          null,
          { timeout: 35_000 },
        );
        await compileUi(app, hashCase.files[0].source);
        return { failure, recoveredDocuments: await app.locator("#file-list button").count() };
      } finally {
        await context.close();
      }
    });
    await check("real watchdog replaces a busy worker and drains only newest pending input", async () => {
      const { app, context } = await open(input.root);
      try {
        const starts = [];
        app.on("worker", (worker) => starts.push(worker.url()));
        await app
          .workers()
          .find((worker) => worker.url().endsWith("/dotnet/worker.mjs"))
          .evaluate(() =>
            setTimeout(() => {
              const end = performance.now() + 20_000;
              while (performance.now() < end) {}
            }, 0),
          );
        await app.waitForTimeout(50);
        const started = Date.now();
        await fillSource(app, "struct OldRequest { long value; };");
        await app.waitForTimeout(350);
        await fillSource(app, "struct NewestRequest { long value; };");
        const editMs = Date.now() - started;
        await app
          .waitForFunction(
            () =>
              [...document.querySelectorAll("#file-list button")].some((value) =>
                value.textContent.includes("NewestRequest"),
              ),
            null,
            { timeout: 20_000 },
          )
          .catch(async (error) => {
            const state = await app.evaluate(() => ({
              state: document.querySelector("#result-state")?.textContent,
              status: document.querySelector("#status")?.textContent,
              diagnostics: document.querySelector("#diagnostics")?.textContent,
              hostError: document.querySelector("#diagnostics-empty")?.textContent,
              names: [...document.querySelectorAll("#file-list button")].map((value) => value.textContent),
            }));
            throw new Error(`${error}; ${JSON.stringify(state)}`);
          });
        const names = await app.locator("#file-list button").allTextContents();
        assert(
          starts.length >= 1 && !names.some((name) => name.includes("OldRequest")),
          "Replacement/stale result handling failed",
        );
        return { recoveredMs: Date.now() - started, editMs, replacements: starts.length, documentNames: names };
      } finally {
        await context.close();
      }
    });
    await check("current timeout is visible and subsequent compilation succeeds", async () => {
      const { app, context } = await open(input.root);
      try {
        await app
          .workers()
          .find((worker) => worker.url().endsWith("/dotnet/worker.mjs"))
          .evaluate(() =>
            setTimeout(() => {
              const end = performance.now() + 20_000;
              while (performance.now() < end) {}
            }, 0),
          );
        await app.waitForTimeout(50);
        const started = Date.now();
        await fillSource(app, hashCase.files[0].source);
        await app.waitForFunction(
          () => document.querySelector("#diagnostics-empty")?.textContent.includes("Compilation timed out"),
          null,
          { timeout: 15_000 },
        );
        const timeoutMs = Date.now() - started;
        assert((await app.locator("#file-list button").count()) === 0, "Timeout retained generated output");
        await app.waitForFunction(
          () => document.querySelector("#status")?.textContent.includes("Compiler ready"),
          null,
          { timeout: 35_000 },
        );
        await compileUi(app, hashCase.files[0].source);
        return { timeoutMs, recoveredDocuments: await app.locator("#file-list button").count() };
      } finally {
        await context.close();
      }
    });
  } else {
    await check("runtime asset failure reports startup error and retries", async () => {
      const context = await browser.newContext();
      const app = await context.newPage();
      try {
        await app.route("**/dotnet/_framework/dotnet.native*.js", (route) => route.abort("failed"));
        await app.goto(input.root);
        await app.locator("#retry").waitFor({ state: "visible", timeout: 35_000 });
        const failure = await app.locator("#status").textContent();
        await app.unroute("**/dotnet/_framework/dotnet.native*.js");
        await app.locator("#retry").click();
        await app.waitForFunction(
          () => document.querySelector("#status")?.textContent.includes("Compiler ready"),
          null,
          { timeout: 35_000 },
        );
        await compileUi(app, hashCase.files[0].source);
        return { failure, recoveredDocuments: await app.locator("#file-list button").count() };
      } finally {
        await context.close();
      }
    });
    const chromiumInput = browser.browserType().name() === "chromium";
    await check(
      chromiumInput
        ? "UI rejects oversized ASCII and UTF-8 edits before posting, then recovers"
        : "UI rejects oversized ASCII and UTF-8 imports while preserving current output",
      async () => {
        const { app, context } = await open(input.root);
        try {
          await app.evaluate(() => {
            globalThis.__verificationPosts = 0;
            const original = Worker.prototype.postMessage;
            Worker.prototype.postMessage = function (...args) {
              globalThis.__verificationPosts++;
              return original.apply(this, args);
            };
          });
          await compileUi(app, hashCase.files[0].source);
          const before = await app.evaluate(() => globalThis.__verificationPosts);
          if (!chromiumInput) {
            const names = await app.locator("#file-list button").allTextContents();
            for (const source of ["x".repeat(262_145), "é".repeat(131_073)]) {
              await app
                .locator("#file")
                .setInputFiles({ name: "oversized.idl", mimeType: "text/plain", buffer: Buffer.from(source, "utf8") });
              await app.waitForFunction(() =>
                document.querySelector("#notice")?.textContent.includes("Could not open IDL"),
              );
              assert(
                JSON.stringify(await app.locator("#file-list button").allTextContents()) === JSON.stringify(names),
                "Rejected import changed generated output",
              );
              assert(!(await app.locator("#download-all").isDisabled()), "Rejected import invalidated valid output");
            }
            assert(
              (await app.evaluate(() => globalThis.__verificationPosts)) === before,
              "Oversized import reached the worker",
            );
            return {
              rejectedImports: 2,
              retainedDocuments: names.length,
              oversizedEditorPaste: "not qualified on this engine",
            };
          }
          for (const source of ["x".repeat(262_145), "é".repeat(131_073)]) {
            await fillSource(app, source);
            await app.waitForFunction(() =>
              document.querySelector("#diagnostics-empty")?.textContent.includes("256 KiB"),
            );
            assert(
              (await app.locator("#diagnostics-empty").textContent()).includes("256 KiB"),
              "Oversized input was not explained",
            );
            assert((await app.locator("#file-list button").count()) === 0, "Oversized input retained output");
          }
          const after = await app.evaluate(() => globalThis.__verificationPosts);
          assert(after === before, "Oversized input reached the worker");
          await compileUi(app, hashCase.files[0].source);
          return {
            rejectedInputs: 2,
            workerRequestsBefore: before,
            workerRequestsAfterRejected: after,
            recoveredDocuments: await app.locator("#file-list button").count(),
          };
        } finally {
          await context.close();
        }
      },
    );
    await check("adapter guards and result cap preserve a usable worker", async () => {
      const { app, context } = await open(input.root);
      try {
        return await app.evaluate(async (fixture) => {
          const worker = new Worker("/dotnet/worker.mjs", { type: "module" });
          await new Promise((resolve, reject) => {
            worker.onmessage = (event) =>
              event.data.type === "ready" ? resolve() : reject(new Error(event.data.message));
            worker.onerror = (event) => reject(new Error(event.message));
          });
          let requestId = 0;
          const send = (extra = {}) =>
            new Promise((resolve, reject) => {
              const timer = setTimeout(() => reject(new Error("Guard verification timed out")), 10_000);
              worker.onmessage = (event) => {
                clearTimeout(timer);
                resolve(event.data);
              };
              worker.postMessage({
                protocolVersion: 1,
                workerGeneration: 1,
                requestId: ++requestId,
                workspaceRevision: requestId,
                profile: "7.7.0",
                files: fixture.files,
                options: {},
                ...extra,
              });
            });
          try {
            const invalid = [];
            for (const extra of [
              { protocolVersion: 0 },
              { profile: "unsupported" },
              { files: [{ ...fixture.files[0], source: "é".repeat(131_073) }] },
            ]) {
              const result = await send(extra);
              if (result.type !== "error") throw new Error("Invalid request produced compiler output");
              invalid.push(result.message);
            }
            const source = Array.from({ length: 129 }, (_, index) => `struct Capped${index} { long value; };`).join(
              "\n",
            );
            const { response } = await send({
              files: [{ path: "/workspace/capped.idl", source, isGenerationRoot: true }],
            });
            if (response.kind !== "limit" || response.success || response.documents.length !== 0)
              throw new Error("Document limit did not reject the complete result");
            const { response: recovered } = await send();
            if (!recovered.success || recovered.documents.length !== 4)
              throw new Error("Guard failure poisoned the worker");
            return {
              rejectedRequests: invalid,
              resultCap: response.error,
              recoveredDocuments: recovered.documents.length,
            };
          } finally {
            worker.terminate();
          }
        }, hashCase);
      } finally {
        await context.close();
      }
    });
  }
  report.sourceStayedLocal = report.requests.every(
    (request) =>
      request.method === "GET" &&
      !request.hasBody &&
      (request.url.startsWith(input.root) || request.url.startsWith(input.nested)),
  );
  report.passed =
    report.checks.every(
      (value) =>
        value.passed && (!value.details?.parity || value.details.parity.results.every((result) => result.passed)),
    ) && report.sourceStayedLocal;
  report.requestCount = report.requests.length;
  delete report.requests;
  return report;
};

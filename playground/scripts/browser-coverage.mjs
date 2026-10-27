import assert from "node:assert/strict";
import { readFile, mkdir, writeFile } from "node:fs/promises";
import { spawn, execFileSync } from "node:child_process";
import { isAbsolute, relative, resolve } from "node:path";
import { pathToFileURL } from "node:url";
import { chromium } from "playwright";
import v8ToIstanbul from "v8-to-istanbul";
import coverage from "istanbul-lib-coverage";
import reportLibrary from "istanbul-lib-report";
import reports from "istanbul-reports";
import { examples } from "../src/examples.ts";

const root = resolve(import.meta.dirname, "../..");
const bundleDir = resolve(import.meta.dirname, "../dist-coverage");
const vite = resolve(import.meta.dirname, "../node_modules/vite/bin/vite.js");
const outputDir = resolve(import.meta.dirname, "../coverage");
const reportPath = resolve(outputDir, "coverage-final.json");
const nodeReportPath = resolve(outputDir, "unit/coverage-final.json");
const summaryPath = resolve(outputDir, "browser-summary.json");
const port = 4175;
const origin = `http://127.0.0.1:${port}`;
const { createCoverageMap } = coverage;

await mkdir(outputDir, { recursive: true });
const server = spawn(
  process.execPath,
  [vite, "preview", "--host", "127.0.0.1", "--port", String(port), "--strictPort", "--outDir", bundleDir],
  {
    cwd: resolve(import.meta.dirname, ".."),
    stdio: ["ignore", "pipe", "pipe"],
  },
);
let serverError = "";
let serverOutput = "";
server.stdout.setEncoding("utf8");
server.stdout.on("data", (data) => {
  serverOutput += data;
});
server.stderr.setEncoding("utf8");
server.stderr.on("data", (data) => {
  serverError += data;
});
let browser;

try {
  await waitForServer();
  browser = await chromium.launch({ headless: true });
  const context = await browser.newContext({ viewport: { width: 1440, height: 1080 }, acceptDownloads: true });
  const page = await context.newPage();
  page.on("dialog", (dialog) => dialog.accept());
  const pageErrors = [];
  page.on("pageerror", (error) => pageErrors.push(String(error)));
  const cdp = await context.newCDPSession(page);
  const primaryScripts = [];
  await cdp.send("Profiler.enable");
  await cdp.send("Profiler.startPreciseCoverage", { callCount: false, detailed: true });
  await page.goto(`${origin}/`);
  await page.waitForFunction(() => document.querySelector("#result-state")?.textContent === "Current", null, {
    timeout: 40_000,
  });

  const combinations = [];
  for (const example of examples) {
    for (const profile of ["7.3.1", "7.7.0"]) {
      await page.locator("#profile").selectOption(profile);
      await waitForTerminal(page);
      await page.locator("#example").selectOption(example.id);
      await page.locator("#load-example").click();
      await page.waitForFunction(
        () => ["Current", "Failed"].includes(document.querySelector("#result-state")?.textContent),
        null,
        { timeout: 15_000 },
      );
      const shouldSucceed = example.id !== "unsupported" && !(example.id === "profiles" && profile === "7.3.1");
      assert.equal(
        (await page.locator("#result-state").textContent()) === "Current",
        shouldSucceed,
        `${example.id}/${profile} had the wrong result state.`,
      );
      combinations.push(`${example.id}/${profile}`);
    }
  }

  await page.locator("#example").selectOption("hello");
  await page.locator("#load-example").click();
  await waitForCurrent(page, "known-success example before editor interaction checks");
  await page.locator("#explorer-splitter").focus();
  await page.keyboard.press("ArrowRight");
  await page.locator("#splitter").focus();
  await page.keyboard.press("ArrowLeft");
  await page.locator("#font-decrease").click();
  await page.locator(".theme-toggle label").filter({ hasText: "Dark" }).click();
  const firstGeneratedTab = page.locator("#output-tabs [role=tab]").first();
  await firstGeneratedTab.focus();
  const firstGeneratedName = await firstGeneratedTab.textContent();
  await page.keyboard.press("ArrowRight");
  assert.notEqual(
    await page.locator('#output-tabs [role="tab"][aria-selected="true"]').textContent(),
    firstGeneratedName,
    "ArrowRight did not move focus to another generated tab.",
  );
  await page.locator("#output-tabs [role=tab]").first().click();
  await page.keyboard.press("End");
  await page.keyboard.press("Home");
  const explorerToggle = page.locator("#explorer-toggle");
  await explorerToggle.click();
  assert.equal(await explorerToggle.getAttribute("aria-expanded"), "false");
  await explorerToggle.click();
  assert.equal(await explorerToggle.getAttribute("aria-expanded"), "true");
  await page.setViewportSize({ width: 800, height: 1080 });
  await page.setViewportSize({ width: 1440, height: 1080 });
  await page.locator("#copy").click();
  await page.locator("#notice-dismiss").click();

  // Exercise generated-tab scrolling with enough output tabs to overflow the strip.
  await page.locator("#example").selectOption("types");
  await page.locator("#load-example").click();
  await waitForCurrent(page, "type showcase before generated-tab scrollbar checks");
  await page.setViewportSize({ width: 800, height: 1080 });
  const tabs = page.locator("#output-tabs");
  const strip = page.locator("#output-tabs-strip");
  const thumb = page.locator("#tab-scroll-thumb");
  assert.equal(await page.locator("#tab-scrollbar").isVisible(), true, "Generated tabs did not overflow the strip.");
  await tabs.evaluate((node) => (node.scrollLeft = 0));
  await thumb.hover();
  const thumbBounds = await thumb.boundingBox();
  assert(thumbBounds, "Generated-tab scrollbar thumb has no visible bounds.");
  await page.mouse.move(thumbBounds.x + 2, thumbBounds.y + 2);
  await page.mouse.down();
  assert((await strip.getAttribute("class")).includes("dragging"), "Pointer down did not start tab dragging.");
  await page.mouse.move(thumbBounds.x + 24, thumbBounds.y + 2);
  await page.mouse.up();
  assert(!(await strip.getAttribute("class")).includes("dragging"), "Pointer up did not stop tab dragging.");
  assert((await tabs.evaluate((node) => node.scrollLeft)) > 0, "Dragging the generated-tab thumb did not scroll tabs.");
  await thumb.focus();
  await page.keyboard.press("End");
  await page.waitForFunction(() => {
    const thumb = document.querySelector("#tab-scroll-thumb");
    return thumb.getAttribute("aria-valuenow") === thumb.getAttribute("aria-valuemax");
  });
  assert.equal(await thumb.getAttribute("aria-valuenow"), await thumb.getAttribute("aria-valuemax"));
  await page.keyboard.press("Home");
  await page.waitForFunction(() => document.querySelector("#tab-scroll-thumb").getAttribute("aria-valuenow") === "0");
  assert.equal(await thumb.getAttribute("aria-valuenow"), "0");
  await page.keyboard.press("PageDown");
  await page.waitForFunction(
    () => Number(document.querySelector("#tab-scroll-thumb").getAttribute("aria-valuenow")) > 0,
  );
  assert(Number(await thumb.getAttribute("aria-valuenow")) > 0, "PageDown did not scroll the tab strip.");
  await page.keyboard.press("PageUp");
  await page.keyboard.press("ArrowRight");
  await page.keyboard.press("ArrowLeft");
  await page.keyboard.press("x");
  await strip.evaluate((node) => {
    const tabs = document.querySelector("#output-tabs");
    const before = tabs.scrollLeft;
    const event = new WheelEvent("wheel", { deltaY: 120, bubbles: true, cancelable: true });
    node.dispatchEvent(event);
    if (!event.defaultPrevented) throw new Error("Unmodified wheel input was not consumed by the generated-tab strip.");
    if (tabs.scrollLeft <= before) throw new Error("Unmodified wheel input did not scroll generated tabs.");
  });
  await strip.evaluate((node) => {
    node.dispatchEvent(new WheelEvent("wheel", { deltaY: 120, ctrlKey: true, bubbles: true, cancelable: true }));
    node.dispatchEvent(new WheelEvent("wheel", { deltaY: 0, bubbles: true, cancelable: true }));
    node.dispatchEvent(
      new WheelEvent("wheel", { deltaY: 1, deltaMode: WheelEvent.DOM_DELTA_LINE, bubbles: true, cancelable: true }),
    );
    node.dispatchEvent(
      new WheelEvent("wheel", { deltaY: 1, deltaMode: WheelEvent.DOM_DELTA_PAGE, bubbles: true, cancelable: true }),
    );
    node.dispatchEvent(new WheelEvent("wheel", { deltaX: 120, deltaY: 1, bubbles: true, cancelable: true }));
  });

  // Both the menu-local and document-level Escape handlers must dismiss generated-file menus.
  await page.locator("#file-list button").first().click({ button: "right" });
  await page.keyboard.press("Escape");
  assert.equal(await page.locator('[role="menu"]').count(), 0, "Escape did not close a generated-file menu.");
  await page.locator("#file-list button").first().click({ button: "right" });
  await page.evaluate(() => document.body.dispatchEvent(new PointerEvent("pointerdown", { bubbles: true })));
  assert.equal(
    await page.locator('[role="menu"]').count(),
    0,
    "Outside pointer input did not close a generated-file menu.",
  );
  await page.locator("#output-tabs [role=tab]").first().click({ button: "right" });
  await page.keyboard.press("Escape");
  assert.equal(await page.locator('[role="menu"]').count(), 0, "Menu-local Escape did not close a generated-tab menu.");
  await page.locator("#output-tabs [role=tab]").first().click({ button: "right" });
  await page.evaluate(() => document.dispatchEvent(new KeyboardEvent("keydown", { key: "Escape", bubbles: true })));
  assert.equal(await page.locator('[role="menu"]').count(), 0, "Document Escape did not close a generated-tab menu.");
  primaryScripts.push(...(await cdp.send("Profiler.takePreciseCoverage")).result);
  await cdp.send("Profiler.startPreciseCoverage", { callCount: false, detailed: true });
  await page.setViewportSize({ width: 1440, height: 1080 });
  await page.locator("#reset").click();
  await page.waitForFunction(() => document.querySelector("#result-state")?.textContent === "Empty");

  await page.locator("#file").setInputFiles({
    name: "coverage.idl",
    mimeType: "text/plain",
    buffer: Buffer.from("// café\r\nstruct Coverage { long value; };\r\n", "utf8"),
  });
  await page.waitForFunction(
    () => document.querySelector("#source-tabs [aria-selected=true]")?.textContent === "coverage.idl",
  );
  await waitForCurrent(page);

  await page.locator('[data-path="/workspace/coverage.idl"]').click({ button: "right" });
  await page.getByRole("menuitem", { name: "Mark as Include-only" }).click();
  await page.waitForFunction(() => document.querySelector("#result-state")?.textContent === "Failed");
  assert.match(await page.locator("#diagnostics-empty").textContent(), /Mark at least one file as Root/);
  primaryScripts.push(...(await cdp.send("Profiler.takePreciseCoverage")).result);
  await cdp.send("Profiler.startPreciseCoverage", { callCount: false, detailed: true });
  await page.locator("#reset").click();
  await page.waitForFunction(() => document.querySelector("#result-state")?.textContent === "Empty");

  await page.locator("#file").setInputFiles({
    name: "coverage.idl",
    mimeType: "text/plain",
    buffer: Buffer.from("// café\r\nstruct Coverage { long value; };\r\n", "utf8"),
  });
  await page.waitForFunction(
    () => document.querySelector("#source-tabs [aria-selected=true]")?.textContent === "coverage.idl",
  );
  await waitForCurrent(page);
  await page.reload();
  await waitForCurrent(page);
  await page.locator("#file").setInputFiles({
    name: "broken.idl",
    mimeType: "text/plain",
    buffer: Buffer.from("struct Broken { long value;\n"),
  });
  await page.waitForFunction(
    () => document.querySelector("#source-tabs [aria-selected=true]")?.textContent === "broken.idl",
  );
  await page.waitForFunction(() => document.querySelector("#result-state")?.textContent === "Failed");
  assert.equal(await page.locator("#output-tabs [role=tab]").count(), 0);
  await page.locator("#diagnostics button").first().click();
  assert.equal(await page.evaluate(() => !!document.activeElement?.closest("#source")), true);
  await page.locator("#reset").click();
  await waitForCurrent(page);
  assert.deepEqual(pageErrors, [], "The production Playground raised uncaught page errors.");

  // Run the workspace scenarios with coverage attached to each isolated page.
  execFileSync(process.execPath, ["playground/scripts/verification/prepare-example-cases.mjs"], { cwd: root });
  execFileSync(
    "dotnet",
    [
      "run",
      "--file",
      "playground/scripts/verification/export-native-baseline.cs",
      "--",
      "output/playwright/example-cases.json",
      "output/playwright/example-native-baseline.json",
    ],
    { cwd: root },
  );
  const template = await readFile(resolve(root, "playground/scripts/verification/verify-workspace.cli.mjs"), "utf8");
  const workspaceInput = {
    phase: "workspace",
    root: `${origin}/`,
    exampleExpected: JSON.parse(
      await readFile(resolve(root, "output/playwright/example-native-baseline.json"), "utf8"),
    ),
  };
  const modulePath = resolve(outputDir, "workspace-coverage.mjs");
  await writeFile(
    modulePath,
    `export default ${template.replace("__WIRELOOM_INPUT__", JSON.stringify(workspaceInput))}`,
  );
  const { default: verifyWorkspace } = await import(pathToFileURL(modulePath).href);
  const sessions = new Map();
  const workspaceScripts = [];
  const previousDirectory = process.cwd();
  process.chdir(root);
  let workspaceResult;
  try {
    workspaceResult = await verifyWorkspace(page, {
      start: async (app) => {
        const session = await app.context().newCDPSession(app);
        await session.send("Profiler.enable");
        await session.send("Profiler.startPreciseCoverage", { callCount: false, detailed: true });
        sessions.set(app, session);
      },
      finish: async (app) => {
        const session = sessions.get(app);
        workspaceScripts.push(...(await session.send("Profiler.takePreciseCoverage")).result);
        await session.send("Profiler.stopPreciseCoverage");
        await session.detach();
      },
    });
  } finally {
    process.chdir(previousDirectory);
  }
  assert(workspaceResult.passed, JSON.stringify(workspaceResult.checks.filter((check) => !check.passed)));
  const raw = await cdp.send("Profiler.takePreciseCoverage");
  raw.result.push(...primaryScripts, ...workspaceScripts);
  await cdp.send("Profiler.stopPreciseCoverage");
  await cdp.detach();
  await context.close();

  const browserMap = createCoverageMap({});
  let mappedScriptCount = 0;
  let mappedSourceCount = 0;
  for (const script of raw.result) {
    if (!script.url.startsWith(`${origin}/assets/`)) continue;
    const bundlePath = resolve(bundleDir, `.${new URL(script.url).pathname}`);
    const bundleRelativePath = relative(bundleDir, bundlePath);
    if (isAbsolute(bundleRelativePath) || bundleRelativePath.startsWith("..")) continue;
    const mapPath = `${bundlePath}.map`;
    let sourceMap;
    try {
      sourceMap = JSON.parse(await readFile(mapPath, "utf8"));
    } catch {
      continue;
    }
    const converter = v8ToIstanbul(bundlePath, 0, { sourceMap: { sourcemap: sourceMap } });
    await converter.load();
    converter.applyCoverage(script.functions);
    const applicationSources = Object.fromEntries(
      Object.entries(converter.toIstanbul()).filter(([sourcePath]) => {
        const normalizedPath = sourcePath.replaceAll("\\", "/");
        return normalizedPath.startsWith(`${root.replaceAll("\\", "/")}/playground/src/`);
      }),
    );
    browserMap.merge(applicationSources);
    mappedSourceCount += Object.keys(applicationSources).length;
    mappedScriptCount++;
  }
  assert(mappedSourceCount > 0, "No Playground source files were mapped into browser coverage.");

  const combined = createCoverageMap(JSON.parse(await readFile(nodeReportPath, "utf8")));
  combined.merge(browserMap);
  const totals = combined.getCoverageSummary().toJSON();
  const summary = {
    engine: `Chromium ${browser.version()}`,
    scenarios: combinations,
    browserScriptsMapped: mappedScriptCount,
    statementCoverage: totals.statements.pct,
    branchCoverage: totals.branches.pct,
    functionCoverage: totals.functions.pct,
    lineCoverage: totals.lines.pct,
    files: combined.files().map((file) => ({
      path: file.slice(root.length + 1),
      ...combined.fileCoverageFor(file).toSummary().toJSON(),
    })),
  };
  await writeFile(reportPath, JSON.stringify(combined.toJSON(), null, 2));
  await writeFile(summaryPath, JSON.stringify(summary, null, 2));
  const reportContext = reportLibrary.createContext({
    dir: outputDir,
    coverageMap: combined,
    defaultSummarizer: "flat",
  });
  reports.create("cobertura", { file: "cobertura-coverage.xml", projectRoot: root }).execute(reportContext);
  console.log(JSON.stringify(summary, null, 2));
  assert(totals.branches.pct >= 70, `Combined branch coverage ${totals.branches.pct}% is below 70%.`);
  assert(totals.lines.pct >= 80, `Combined line coverage ${totals.lines.pct}% is below 80%.`);
} finally {
  await browser?.close();
  server.kill();
}

async function waitForCurrent(page, scope = "current compiler result") {
  try {
    await page.waitForFunction(() => document.querySelector("#result-state")?.textContent === "Current", null, {
      timeout: 30_000,
    });
  } catch (error) {
    const state = await page.locator("#result-state").textContent();
    const status = await page.locator("#status").textContent();
    throw new Error(`Timed out waiting for ${scope}; state=${state}; status=${status}; ${String(error)}`);
  }
}

async function waitForTerminal(page) {
  await page.waitForFunction(
    () => ["Current", "Failed"].includes(document.querySelector("#result-state")?.textContent),
    null,
    { timeout: 30_000 },
  );
}

async function waitForServer() {
  for (let attempt = 0; attempt < 100; attempt++) {
    if (server.exitCode !== null) throw new Error(`Vite preview exited with ${server.exitCode}: ${serverError}`);
    try {
      const response = await fetch(`${origin}/`);
      if (response.ok) return;
    } catch (error) {
      if (serverOutput.includes("Local:"))
        throw new Error(`Vite preview started but is not reachable: ${serverOutput}; ${String(error)}; ${serverError}`);
    }
    await new Promise((resolve) => setTimeout(resolve, 200));
  }
  throw new Error(`Timed out waiting for the coverage preview server: ${serverOutput}; ${serverError}`);
}

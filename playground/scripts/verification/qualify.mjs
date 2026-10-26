import assert from "node:assert/strict";
import { createHash } from "node:crypto";
import { spawn, execFileSync } from "node:child_process";
import { readFile, writeFile, mkdir, readdir, open, unlink } from "node:fs/promises";
import { resolve, relative } from "node:path";
import { chromium, firefox, webkit } from "playwright";

const root = resolve(import.meta.dirname, "../../..");
const client = resolve(root, "playground");
const output = resolve(root, "output/playwright");

// CLI templates also save screenshots using repository-relative paths.
process.chdir(root);

const engine = process.argv[2] ?? "chromium";

assert(["chromium", "msedge", "firefox", "webkit"].includes(engine), "Choose chromium, msedge, firefox, or webkit");

const phases = (
  process.argv[3] ??
  (engine === "chromium" || engine === "msedge"
    ? "parity,corpus,corpus-ui,desktop,ui,recovery,ui-faults,faults,limits"
    : "parity,corpus,corpus-ui,desktop,faults,limits")
).split(",");

const started = new Date().toISOString();
const servers = [];

let browser;
let log = "";

const report = { engine, started, phases: [], passed: false };

await mkdir(output, { recursive: true });

const lockPath = resolve(output, "qualification.lock");
const lock = await open(lockPath, "wx").catch(() => {
  throw new Error(
    "Another qualification owns output/playwright/qualification.lock. Remove a stale lock only after its runner has stopped.",
  );
});

await lock.writeFile(`${process.pid}\n`);

const run = (command, args, env = process.env) =>
  new Promise((resolveRun, reject) => {
    const child = spawn(command, args, { cwd: root, env, stdio: ["ignore", "pipe", "pipe"] });
    child.stdout.on("data", (chunk) => {
      log += chunk;
    });
    child.stderr.on("data", (chunk) => {
      log += chunk;
    });
    child.once("error", reject);
    child.once("exit", (code) =>
      code === 0
        ? resolveRun()
        : reject(new Error(`${command} ${args.join(" ")} exited ${code}; see qualification-${engine}.log`)),
    );
  });

const sha256 = (bytes) => createHash("sha256").update(bytes).digest("hex");
const git = (...args) => execFileSync("git", args, { cwd: root });
const hashTree = async (directory) => {
  const files = [];

  for (const entry of await readdir(directory, { withFileTypes: true })) {
    const path = resolve(directory, entry.name);
    if (entry.isDirectory()) files.push(...(await hashTree(path)));
    else files.push({ path: relative(root, path).replaceAll("\\", "/"), sha256: sha256(await readFile(path)) });
  }

  return files.sort((a, b) => a.path.localeCompare(b.path));
};

const serve = async (port, bundle, base) => {
  const server = spawn(
    process.execPath,
    [
      resolve(client, "node_modules/vite/bin/vite.js"),
      "preview",
      "--host",
      "127.0.0.1",
      "--port",
      String(port),
      "--strictPort",
      "--outDir",
      bundle,
      "--base",
      base,
    ],
    { cwd: client, stdio: ["ignore", "pipe", "pipe"] },
  );

  servers.push(server);

  server.stdout.on("data", (chunk) => {
    log += chunk;
  });

  server.stderr.on("data", (chunk) => {
    log += chunk;
  });

  let failure;

  server.once("error", (error) => {
    failure = error;
  });

  for (let attempt = 0; attempt < 100; attempt++) {
    if (failure) throw failure;

    assert(server.exitCode === null, `Preview server exited; check that port ${port} is free`);

    try {
      if ((await fetch(`http://127.0.0.1:${port}${base}`)).ok) return;
    } catch {
      /* Wait for this owned server to bind. */
    }

    await new Promise((resume) => setTimeout(resume, 100));
  }

  throw new Error(`Preview server did not start on port ${port}`);
};

try {
  // Export with the native platform MD5, independently of the browser provider.
  await run(process.execPath, ["playground/scripts/verification/prepare-corpus-cases.mjs"]);
  await run(process.execPath, ["playground/scripts/verification/prepare-example-cases.mjs"]);

  for (const [cases, baseline] of [
    ["playground/scripts/verification/cases.json", "native-baseline.json"],
    ["output/playwright/corpus-cases.json", "corpus-native-baseline.json"],
    ["output/playwright/example-cases.json", "example-native-baseline.json"],
  ]) {
    await run("dotnet", [
      "run",
      "--file",
      "playground/scripts/verification/export-native-baseline.cs",
      "--",
      cases,
      `output/playwright/${baseline}`,
    ]);
  }

  report.sourceRevision = git("rev-parse", "HEAD").toString().trim();
  report.sourceStatus = git("status", "--porcelain").toString();
  report.trackedDiffSha256 = sha256(git("diff", "HEAD", "--binary"));
  report.node = process.version;
  report.sdk = execFileSync("dotnet", ["--version"], { cwd: root, encoding: "utf8" }).trim();
  report.inputs = JSON.parse(await readFile(resolve(output, "corpus-inputs.json"), "utf8"));
  report.bundles = [...(await hashTree(resolve(client, "dist"))), ...(await hashTree(resolve(client, "dist-subpath")))];
  report.verificationSources = await hashTree(import.meta.dirname);
  report.baselines = [];

  for (const path of ["native-baseline.json", "corpus-native-baseline.json", "example-native-baseline.json"]) {
    report.baselines.push({ path, sha256: sha256(await readFile(resolve(output, path))) });
  }

  // Dedicated ports avoid testing a preview from another worktree by accident.
  await serve(4183, "dist", "/");
  await serve(4184, "dist-subpath", "/playground/");

  browser = await { chromium, msedge: chromium, firefox, webkit }[engine].launch({
    headless: true,
    ...(engine === "msedge" ? { channel: "msedge" } : {}),
  });

  report.browserVersion = browser.version();

  const page = await browser.newPage();

  for (const phase of phases) {
    console.log(`${engine}: ${phase}`);

    await run(process.execPath, ["playground/scripts/verification/prepare-browser-verification.mjs", phase], {
      ...process.env,
      WIRELOOM_VERIFY_ROOT: "http://127.0.0.1:4183/",
      WIRELOOM_VERIFY_NESTED: "http://127.0.0.1:4184/playground/",
    });

    const source = await readFile(resolve(output, `run-${phase}.mjs`), "utf8");
    const { default: verify } = await import(
      `data:text/javascript;base64,${Buffer.from(`export default ${source}`).toString("base64")}`
    );

    let deadline;
    let result;

    try {
      result = await Promise.race([
        verify(page),
        new Promise((_, reject) => {
          deadline = setTimeout(
            () => reject(new Error(`${phase} exceeded the 180-second verification deadline`)),
            180_000,
          );
        }),
      ]);
    } finally {
      clearTimeout(deadline);
    }

    report.phases.push(result);
    console.log(`${engine}: ${phase}: ${result.passed ? "PASS" : "FAIL"} (${result.checks.length} checks)`);

    await writeFile(resolve(output, `qualification-${engine}.json`), JSON.stringify(report, null, 2));
  }

  report.passed =
    report.phases.length === phases.length &&
    report.phases.every((phase) => phase.passed === true && phase.checks.length > 0);
} catch (error) {
  report.error = String(error);
  console.error(error);
} finally {
  await browser?.close();

  for (const server of servers) server.kill();

  report.finished = new Date().toISOString();

  await writeFile(resolve(output, `qualification-${engine}.json`), JSON.stringify(report, null, 2));
  await writeFile(resolve(output, `qualification-${engine}.log`), log);
  await lock.close();
  await unlink(lockPath);
}

if (!report.passed) process.exitCode = 1;

console.log(resolve(output, `qualification-${engine}.json`));

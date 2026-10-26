import assert from "node:assert/strict";
import { spawn } from "node:child_process";
import { mkdir, readFile, writeFile } from "node:fs/promises";
import { resolve } from "node:path";
import { get as httpGet } from "node:http";
import { get as httpsGet } from "node:https";
import { brotliDecompressSync, gunzipSync, inflateSync } from "node:zlib";
import { chromium } from "playwright";
import { strFromU8, unzipSync } from "fflate";
import { sha256, verifyManifest } from "./pages-manifest.mjs";

const client = resolve(import.meta.dirname, "..");
const directory = resolve(client, process.env.PLAYGROUND_PAGES_DIRECTORY ?? "dist-pages");
const reportPath = resolve(client, "../output/playwright/pages-smoke.json");
const report = { started: new Date().toISOString(), passed: false, assets: [], checks: [] };
let server;
let browser;
const pause = (ms) => new Promise((resume) => setTimeout(resume, ms));
const request = (url) => fetch(url, { cache: "no-store", signal: AbortSignal.timeout(15_000) });
const asset = (url) =>
  new Promise((resolveAsset, reject) => {
    const get = url.protocol === "https:" ? httpsGet : httpGet;
    const request = get(
      url,
      { headers: { "Accept-Encoding": "identity", "Cache-Control": "no-cache" }, signal: AbortSignal.timeout(15_000) },
      (response) => {
        const chunks = [];
        response.on("data", (chunk) => chunks.push(chunk));
        response.on("error", reject);
        response.on("end", () =>
          resolveAsset({ status: response.statusCode, headers: response.headers, bytes: Buffer.concat(chunks) }),
        );
      },
    );
    request.on("error", reject);
  });
const check = async (name, action) => {
  const details = await action();
  report.checks.push({ name, passed: true, details });
};

try {
  const manifest = await verifyManifest(directory, process.env.EXPECTED_SOURCE_SHA);
  const remote = process.argv[2];
  const url = new URL(remote ?? `http://127.0.0.1:4185${manifest.base}`);
  assert.equal(url.pathname, manifest.base, "Site URL must match the artifact base path, including the trailing slash");
  assert(!url.search && !url.hash, "Use the canonical site URL without query or fragment");
  if (remote) assert.equal(url.protocol, "https:", "Public verification requires HTTPS");
  report.url = url.href;
  report.sourceRevision = manifest.sourceRevision;
  report.manifestSha256 = sha256(await readFile(resolve(directory, "deployment.json")));

  if (!remote) {
    server = spawn(
      process.execPath,
      [
        "node_modules/vite/bin/vite.js",
        "preview",
        "--host",
        "127.0.0.1",
        "--port",
        "4185",
        "--strictPort",
        "--outDir",
        directory,
        "--base",
        manifest.base,
      ],
      { cwd: client, stdio: ["ignore", "ignore", "pipe"] },
    );
    let serverError;
    server.on("error", (error) => {
      serverError = error;
    });
    server.stderr.on("data", (bytes) => {
      report.serverLog = `${report.serverLog ?? ""}${bytes}`;
    });
    for (let attempt = 0; attempt < 50; attempt++) {
      if (serverError) throw serverError;
      assert(server.exitCode === null, "Owned Pages preview exited; check that port 4185 is free");
      try {
        if ((await request(new URL("deployment.json", url))).ok) break;
      } catch {
        /* Wait for the owned server to bind. */
      }
      await pause(100);
    }
  }

  // Pages/CDN propagation can briefly expose the previous complete deployment.
  await check("expected deployment is publicly readable", async () => {
    let failure;
    for (let attempt = 0; attempt < (remote ? 24 : 1); attempt++) {
      try {
        const response = await request(new URL("deployment.json", url));
        assert(response.ok, `Manifest returned HTTP ${response.status}`);
        assert.deepEqual(await response.json(), manifest, "The host is still serving a different deployment");
        return { revision: manifest.sourceRevision, base: manifest.base };
      } catch (error) {
        failure = error;
        if (remote && attempt < 23) await pause(5_000);
      }
    }
    throw failure;
  });

  await check("all deployed bytes and WebAssembly/module MIME types match", async () => {
    for (let start = 0; start < manifest.files.length; start += 8) {
      await Promise.all(
        manifest.files.slice(start, start + 8).map(async (file) => {
          const response = await asset(new URL(file.path, url));
          assert.equal(response.status, 200, `${file.path}: HTTP ${response.status}`);
          let bytes = response.bytes;
          const encoding = response.headers["content-encoding"];
          // Compare precompressed files as stored; decode transport compression of ordinary assets.
          if (!/\.(br|gz)$/.test(file.path) && encoding && encoding !== "identity") {
            const decode = { br: brotliDecompressSync, gzip: gunzipSync, deflate: inflateSync }[encoding];
            assert(decode, `Unsupported content encoding: ${encoding}`);
            bytes = decode(bytes);
          }
          assert.equal(bytes.length, file.bytes, `${file.path}: changed length`);
          assert.equal(sha256(bytes), file.sha256, `${file.path}: changed bytes`);
          const type = response.headers["content-type"] ?? "";
          if (file.path.endsWith(".wasm")) assert.match(type, /^application\/wasm(?:;|$)/i, file.path);
          if (/\.m?js$/.test(file.path))
            assert.match(type, /^(?:text|application)\/(?:javascript|ecmascript)(?:;|$)/i, file.path);
          report.assets.push({
            path: file.path,
            contentType: type,
            encoding,
            cacheControl: response.headers["cache-control"],
            sha256: file.sha256,
          });
        }),
      );
    }
    return { files: manifest.files.length, bytes: manifest.files.reduce((sum, file) => sum + file.bytes, 0) };
  });

  browser = await chromium.launch({ headless: true });
  report.browserVersion = browser.version();
  const context = await browser.newContext({ acceptDownloads: true });
  await context.grantPermissions(["clipboard-read", "clipboard-write"]);
  const page = await context.newPage();
  page.on("dialog", (dialog) => dialog.accept());
  page.setDefaultTimeout(40_000);
  const errors = [];
  page.on("pageerror", (error) => errors.push(String(error)));
  page.on("requestfailed", (request) => errors.push(`${request.url()}: ${request.failure()?.errorText}`));
  const current = () => page.waitForFunction(() => document.querySelector("#result-state")?.textContent === "Current");

  await check("cold sample load, revision, and preview scope", async () => {
    const started = Date.now();
    await page.goto(url.href);
    await current();
    assert((await page.locator("#status").textContent()).includes("Compiler ready"));
    assert.equal(await page.locator("#example").inputValue(), "hello");
    const footer = await page.locator("#version").textContent();
    assert(footer.includes(manifest.version) && footer.includes(manifest.displayRevision), "Footer revision mismatch");
    assert((await page.locator(".page-footer").textContent()).includes("Source preview only"));
    assert(Number(await page.locator("#document-count").textContent()) > 0);
    return { startupMs: Date.now() - started, footer };
  });

  for (const profile of ["7.3.1", "7.7.0"]) {
    await check(`Hello sample generates under RTI profile ${profile}`, async () => {
      await page.locator("#profile").selectOption(profile);
      await current();
      await page.locator("#copy").click();
      const text = await page.evaluate(() => navigator.clipboard.readText());
      assert(text.includes("Reading") && text.includes("namespace Hello"), "Hello output was not generated");
      assert.equal(await page.locator("#diagnostic-count").textContent(), "0");
      return { profile };
    });
  }

  await check("copy, individual download, and ZIP preserve generated text", async () => {
    await page.locator("#copy").click();
    const copied = await page.evaluate(() => navigator.clipboard.readText());
    const downloading = page.waitForEvent("download");
    await page.locator("#download").click();
    const download = await downloading;
    const bytes = await readFile(await download.path());
    assert.equal(bytes.toString("utf8").replace(/\r\n/g, "\n"), copied.replace(/\r\n/g, "\n"));
    const zipping = page.waitForEvent("download");
    await page.locator("#download-all").click();
    const zip = await zipping;
    const entries = unzipSync(await readFile(await zip.path()));
    assert.equal(strFromU8(entries[download.suggestedFilename()]), bytes.toString("utf8"));
    assert.equal(Object.keys(entries).length, Number(await page.locator("#document-count").textContent()));
    return { file: download.suggestedFilename(), zip: zip.suggestedFilename(), documents: Object.keys(entries).length };
  });

  await check("reload starts the compiler and restores the sample", async () => {
    const started = Date.now();
    await page.reload();
    await current();
    return { startupMs: Date.now() - started };
  });

  await check("invalid IDL exposes diagnostics and disables exports", async () => {
    await page.locator("#file").setInputFiles({
      name: "invalid.idl",
      mimeType: "text/plain",
      buffer: Buffer.from("struct Invalid { Missing value; };"),
    });
    await page.waitForFunction(() => document.querySelector("#result-state")?.textContent === "Failed");
    assert(Number(await page.locator("#diagnostic-count").textContent()) > 0);
    for (const id of ["copy", "download", "download-all"]) assert(await page.locator(`#${id}`).isDisabled());
    assert.equal(await page.locator("#document-count").textContent(), "0");
    return { diagnostics: await page.locator("#diagnostics").textContent() };
  });
  assert.deepEqual(errors, [], "Browser runtime or network errors occurred");
  report.passed = true;
} catch (error) {
  report.error = String(error.stack ?? error);
  console.error(error);
  process.exitCode = 1;
} finally {
  await browser?.close();
  server?.kill();
  report.finished = new Date().toISOString();
  await mkdir(resolve(reportPath, ".."), { recursive: true });
  await writeFile(reportPath, `${JSON.stringify(report, null, 2)}\n`);
  console.log(`${report.passed ? "PASS" : "FAIL"}: ${reportPath}`);
}

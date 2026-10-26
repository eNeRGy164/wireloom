import assert from "node:assert/strict";
import { access, readFile, readdir } from "node:fs/promises";
import { resolve } from "node:path";

const root = resolve(import.meta.dirname, "..");
const cases = process.argv[2]
  ? [{ directory: process.argv[2], base: process.argv[3] }]
  : [
      { directory: "dist", base: "/" },
      { directory: "dist-subpath", base: "/playground/" },
    ];

for (const testCase of cases) {
  const output = resolve(root, testCase.directory);
  const html = await readFile(resolve(output, "index.html"), "utf8");
  const scripts = [...html.matchAll(/<script[^>]+src="([^"]+)"/g)].map((match) => match[1]);
  assert.equal(scripts.length, 1, `${testCase.directory} must include one client entry point`);
  assert.ok(scripts[0].startsWith(testCase.base), `${scripts[0]} must use base ${testCase.base}`);
  const assets = await readdir(resolve(output, "assets"));
  const client = (
    await Promise.all(
      assets.filter((name) => name.endsWith(".js")).map((name) => readFile(resolve(output, "assets", name), "utf8")),
    )
  ).join("\n");
  assert.ok(
    client.includes(`${testCase.base}dotnet/worker.mjs`),
    "client must request the worker under the configured base path",
  );
  await access(resolve(output, "dotnet/worker.mjs"));
  await access(resolve(output, "dotnet/md5.mjs"));
  await access(resolve(output, "dotnet/_framework/dotnet.js"));
  await access(resolve(output, "third-party-notices.txt"));
  assert.ok(
    assets.some((name) => name.startsWith("editor.worker-") && name.endsWith(".js")),
    "Monaco editor worker must ship locally",
  );
  const worker = await readFile(resolve(output, "dotnet/worker.mjs"), "utf8");
  assert.ok(worker.includes('from "./_framework/dotnet.js"'));
  assert.ok(worker.includes('from "./md5.mjs"'));
  const assetCount = await countFiles(resolve(output, "dotnet"));
  assert.ok(assetCount > 1, "published .NET runtime files must be copied into the static bundle");
  process.stdout.write(`${testCase.directory}: base ${testCase.base}, ${assetCount} .NET assets present\n`);
}

async function countFiles(directory) {
  const entries = await readdir(directory, { withFileTypes: true });
  let count = 0;
  for (const entry of entries) {
    if (entry.isDirectory()) {
      count += await countFiles(resolve(directory, entry.name));
    } else {
      count++;
    }
  }
  return count;
}

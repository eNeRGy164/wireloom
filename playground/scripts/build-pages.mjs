import assert from "node:assert/strict";
import { execFileSync } from "node:child_process";
import { readFile, writeFile } from "node:fs/promises";
import { resolve } from "node:path";
import { inventory, validateBase, verifyManifest } from "./pages-manifest.mjs";

const client = resolve(import.meta.dirname, "..");
const root = resolve(client, "..");
const git = (...args) => execFileSync("git", args, { cwd: root, encoding: "utf8" }).trim();
const sourceRevision = git("rev-parse", "HEAD");
const dirty = git("status", "--porcelain").length > 0;
if (process.env.CI) assert(!dirty, "Pages CI must build a clean source revision");
const base = validateBase(process.env.PLAYGROUND_BASE_PATH ?? "/wireloom/");
const output = resolve(client, "dist-pages");
const props = await readFile(resolve(root, "Directory.Build.props"), "utf8");
const version = props.match(/<WireloomVersion>([^<]+)<\/WireloomVersion>/)?.[1];

// Reuse the Release runtime produced by npm run build. Never ship the coverage bundle.
execFileSync(process.execPath, ["node_modules/vite/bin/vite.js", "build", "--base", base, "--outDir", output], {
  cwd: client,
  stdio: "inherit",
});
execFileSync(process.execPath, ["scripts/verify-bundles.mjs", "dist-pages", base], {
  cwd: client,
  stdio: "inherit",
});

const manifest = {
  schemaVersion: 1,
  sourceRevision,
  displayRevision: `${git("rev-parse", "--short", "HEAD")}${dirty ? "-dirty" : ""}`,
  version,
  base,
  node: process.version,
  sdk: execFileSync("dotnet", ["--version"], { cwd: root, encoding: "utf8" }).trim(),
  files: await inventory(output),
};
await writeFile(resolve(output, "deployment.json"), `${JSON.stringify(manifest, null, 2)}\n`);
await verifyManifest(output, sourceRevision);
console.log(`Pages candidate: ${sourceRevision} at ${base}, ${manifest.files.length} files`);
